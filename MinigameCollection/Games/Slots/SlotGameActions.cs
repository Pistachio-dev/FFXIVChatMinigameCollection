using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using Model.Base;
using System;
using System.Text.RegularExpressions;

namespace MinigameCollection.Games.Slots
{
    internal class SlotGameActions : IDisposable
    {
        private readonly GameHost host;
        private readonly IChatOutput chatOutput;
        private readonly SlotsGameState gameState;
        private readonly BankActions bank;
        private readonly RollTracker rollTracker;
        private readonly Regex BetRegex = new Regex("bet ([0-9\\.,]+)([km]?)");

        public SlotGameActions(GameHost host, IChatOutput chatOutput, SlotsGameState gameState, BankActions bank, RollTracker rollTracker, IClientChatGui chatGui)
        {
            this.host = host;
            this.chatOutput = chatOutput;
            this.gameState = gameState;
            this.bank = bank;
            this.rollTracker = rollTracker;
        }


        public void Bet(MGPlayer player, long amount)
        {
            if (gameState.Stage != SlotGameStage.Idle)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, the slot machine is buy right now.{amount.Formatted()}");
            }
            if (player.Bank.Stored < amount)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, you don't have enough funds stored to bet {amount.Formatted()}");
                return;
            }

            gameState.Bet = amount;
            gameState.Player = player;
            bank.Draw(player, amount);
        }

        private void SetupRolls()
        {
            for (int i = 0; i < 3; i++)
            {
                rollTracker.QueueExpectedRoll(host.GetHostPlayerFullName(), AcceptedRollType.Dice, 999, OnRoll);
            }
        }

        private void OnRoll(DiceRoll result)
        {
            gameState.Results[gameState.ResultCount] = result.RollResult;
            gameState.ResultCount += 1;

            if (gameState.ResultCount == 3)
            {
                gameState.Stage = SlotGameStage.ShowingResult;
                OutputAndBankResult();
            }
            
        }

        private void OutputAndBankResult()
        {
            Plugin.Log.Info($"Results: {gameState.Results.Humanize()}");
            gameState.Stage = SlotGameStage.Idle;
        }

        private void OnMessageDelegate(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
