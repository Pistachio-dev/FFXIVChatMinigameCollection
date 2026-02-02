using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using DalamudBasics.Targeting;
using ECommons;
using ECommons.Automation;
using ECommons.Automation.UIInput;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI;
using FFXIVClientStructs.FFXIV.Component.GUI;
using MinigameCollection.Bank;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace MinigameCollection.Trader
{
    public class TradingManager : IDisposable
    {
        private readonly IGameGui gameGui;
        private readonly IAddonLifecycle addonLifecycle;
        private readonly IObjectTable objectTable;
        private readonly IChatGui chatGui;
        private readonly IPlayerState playerState;
        private readonly IChatOutput chatOutput;
        private readonly ITargetingService targeting;
        private readonly IFramework framework;
        private readonly GameHost host;
        private readonly BankActions bankActions;
        private TradeType currentTradeType;
        private MGTradeStatus currentTradeStatus;
        private long preTransactionGil; // How much gil you have before the trade
        private bool wasLastTransactionCompleted = false;
        private string lastTradeTargetName = string.Empty;


        public TradingManager(IChatGui chatGui, IObjectTable objectTable, IPlayerState playerState, IChatOutput chatOutput, ITargetingService targeting, IFramework framework,
            GameHost host, BankActions bankActions)
        {
            this.chatGui = chatGui;
            this.objectTable = objectTable;
            this.playerState = playerState;
            this.chatOutput = chatOutput;
            this.targeting = targeting;
            this.framework = framework;
            this.host = host;
            this.bankActions = bankActions;
        }

        public void Attach()
        {
            framework.Update += Update;
        }

        public void Dispose()
        {
            framework.Update -= Update;
        }

        private enum TradeType
        {
            BuyIn,
            CashOut,
            None
        }

        private enum MGTradeStatus
        {
            None,
            RequestSent,
            TransactionOngoing,
        }

        public unsafe void OnTransactionStart()
        {
            if (objectTable.LocalPlayer == null)
            {
                throw new Exception("Local player is null when processing a transaction");
            }
            lastTradeTargetName = targeting.GetTargetName();
            preTransactionGil = GetGil();            
        }

        public void OnTransactionDetected()
        {
            currentTradeStatus = MGTradeStatus.TransactionOngoing;
            if (currentTradeType == TradeType.CashOut && targeting.GetTarget() != null && targeting.GetTarget() is IPlayerCharacter player)
            {
                var tradedPlayer = host.Players.GetPlayer(player.GetFullName());
                if (tradedPlayer != null)
                {
                    SetOutgoing(Math.Min(tradedPlayer.Bank.Stored, 1000000));
                }                
            }
        }

        public void OnTransactionEnd()
        {
            if (targeting.GetTarget() != null && targeting.GetTarget() is IPlayerCharacter player)
            {
                var tradedPlayer = host.Players.GetPlayer(player.GetFullName());
                var gilDifference = GetGilDifference();
                if (tradedPlayer != null)
                {
                    bankActions.AddStored(tradedPlayer, gilDifference);
                }
                
                wasLastTransactionCompleted = gilDifference != 0;

                if (wasLastTransactionCompleted)
                {
                    if (tradedPlayer != null && lastTradeTargetName == tradedPlayer.FullName)
                    {
                        if (currentTradeType == TradeType.BuyIn)
                        {
                            chatGui.Print("Auto trading again for buy-in");
                            StartBuyIn(tradedPlayer);
                        }
                        else if (currentTradeType == TradeType.CashOut && tradedPlayer.Bank.Stored > 0)
                        {
                            chatGui.Print("Auto trading again for cash-out");
                            StartCashOut(tradedPlayer);
                        }
                    }
                }
            }
        }


        public void OnTransactionAbort()
        {
            currentTradeStatus = MGTradeStatus.None;
            wasLastTransactionCompleted = false;
        }

        // Positive means the croupier got more money now
        private long GetGilDifference()
        {
            var newGil = GetGil();
            chatGui.Print($"Previous:  {preTransactionGil} New: {newGil}");
            long difference = newGil - preTransactionGil;
            if (difference > 0)
            {
                chatGui.Print($"{difference.Formatted()} gil received.");
            }
            else if (difference < 0)
            {
                chatGui.Print($"{(difference * -1).Formatted()} gil cashed out");
            }
            else
            {
                chatGui.Print($"Trade cancelled, or 0 gil traded.");
            }

            return difference;
        }

        public void StartBuyIn(MGPlayer player)
        {
            if(!targeting.TargetPlayer(player.FullName)){
                chatGui.PrintError($"Could not select player {player.FullName}. Buy In aborted");
            }

            chatOutput.WriteChat($"Trading for buy in. Current funds: {player.Bank.Stored.Formatted()}");
            chatOutput.WriteCommand("/trade");
            currentTradeStatus = MGTradeStatus.RequestSent;
            currentTradeType = TradeType.BuyIn;
            
        }

        public void StartCashOut(MGPlayer player)
        {
            if (!targeting.TargetPlayer(player.FullName))
            {
                chatGui.PrintError($"Could not select player {player.FullName}. Cash out aborted");
            }
            chatOutput.WriteChat($"Trading for cash out. Current funds: {player.Bank.Stored.Formatted()}");
            chatOutput.WriteCommand("/trade");
            currentTradeStatus = MGTradeStatus.RequestSent;
            currentTradeType = TradeType.CashOut;
        }

        public unsafe void Update(IFramework framework)
        {
            if (currentTradeStatus == MGTradeStatus.None)
            {
                return;
            }
            var invManager = InventoryManager.Instance();
            switch (invManager->TradeLocalState)
            {
                case TradeState.SelectingTradeGoods:
                    if (currentTradeStatus == MGTradeStatus.RequestSent)
                    {
                        OnTransactionStart();
                        OnTransactionDetected();
                        currentTradeStatus = MGTradeStatus.TransactionOngoing;
                    }                    
                    break;
                case TradeState.NotTrading:
                    // Trade finished or cancelled
                    if (currentTradeStatus != MGTradeStatus.None)
                    {
                        currentTradeStatus = MGTradeStatus.None;
                        OnTransactionEnd();
                    }
                    else
                    {
                        // No transaction ongoing, nothing to cancel.
                    }
                        break;
                default:
                    break;
            }
        }

        private unsafe long GetGil()
        {
            return (long)(InventoryManager.Instance()->GetGil());
        }

        public TradingManager(IGameGui gameGui, IAddonLifecycle addonLifecycle)
        {
            this.gameGui = gameGui;
            this.addonLifecycle = addonLifecycle;
        }

        public unsafe void PrintInfo()
        {
            if (GenericHelpers.TryGetAddonByName<AddonTrade>("Trade", out var addon))
            {
                Plugin.Log.Warning("Trade is open");

                var localGilTextNode = GetOutgoingGilTextCmp(addon);
                var text = localGilTextNode->GetText();
                Plugin.Log.Warning($"Text component get: {text}");
                var textOutput = GetIncomingGilTextCmp(addon);
                Plugin.Log.Warning($"Incoming gil: " + textOutput->GetText());

                GetTradeStatus();
                return;
            }            

            Plugin.Log.Warning("Trade NOT open");
        }

        public unsafe void SelectYes()
        {            
            //if (GenericHelpers.TryGetAddonByName<AddonSelectYesno>("SelectYesNo", out var addon)){
            //    if (addon == null) Plugin.Log.Error("Could not get yesno addon");
            //    var yesNoAddon = new AddonMaster.SelectYesno(addon);
            //    yesNoAddon.Yes();
            //}
        }

        public unsafe void SelectYesBruteForce()
        {
            //if (GenericHelpers.TryGetAddonByName<AddonSelectYesno>("SelectYesNo", out var addon))
            //{
            //    var buttonsContainer = addon->GetNodeById(6)->GetAsAtkComponentWindow();
            //    var yesButtonContainer = addon->GetNodeById(7);
            //    var yesButtonNode = addon->GetNodeById(8);
            //    var yesButton = yesButtonContainer->GetAsAtkComponentButton();
            //    yesButton->ClickAddonButton(yesButton->AtkComponentBase);
            //}
        }

        private unsafe bool GetTradeStatus()
        {
            var inventoryManager = InventoryManager.Instance();
            Plugin.Log.Warning($"Local trade state: {inventoryManager->TradeLocalState}");
            Plugin.Log.Warning($"Remote trade state: {inventoryManager->TradeRemoteState}");
            return true;
        }

        private unsafe void SetOutgoing(long amount)
        {
            var inventoryManager = InventoryManager.Instance();
            inventoryManager->SetTradeGilAmount((uint)amount);

            Plugin.Log.Warning("Cash out auto-amount set");
        }

        private unsafe AtkTextNode* GetOutgoingGilTextCmp(AddonTrade* addon)
        {
            var localSection = addon->GetNodeById(6);
            if (localSection == null) return null;

            var gilSubsection = addon->GetNodeById(13);
            if (gilSubsection == null) return null;

            var button = gilSubsection->GetAsAtkComponentButton();
            if (button == null) return null;

            var textNode = button->GetTextNodeById(2);

            return textNode;
        }

        private unsafe AtkTextNode* GetIncomingGilTextCmp(AddonTrade* addon)
        {
            var localSection = addon->GetNodeById(16);
            if (localSection == null) return null;

            var gilTextNode = addon->GetTextNodeById(31);
            if (gilTextNode == null) return null;            

            return gilTextNode;

        }
    }
}
