using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using MinigameCollection.Save;
using Model.Base;
using static FFXIVClientStructs.FFXIV.Client.UI.AddonAirShipExploration;

namespace MinigameCollection.Bank
{
    public class BankActions
    {
        private readonly IChatOutput chatOutput;
        private readonly IClientChatGui chatGui;
        private readonly SaveManager saveManager;
        private readonly PlayerSet playerSet;

        public BankActions(IChatOutput chatOutput, IClientChatGui chatGui, SaveManager saveManager, PlayerSet playerSet)
        {
            this.chatOutput = chatOutput;
            this.chatGui = chatGui;
            this.saveManager = saveManager;
            this.playerSet = playerSet;
        }

        // Negative values to substract
        // Yes this lets you get into debt
        public void AddStored(MGPlayer player, long amount)
        {
            player.Bank.Stored += amount;
            DualPrint($"{amount.Formatted()} added to {player.FullName}'s bank.");
            saveManager.Save();
            
        }

        public void SetStored(MGPlayer player, long amount)
        {
            DualPrint($"{player.FullName} bank is set to {amount.Formatted()} gil");
            player.Bank.Stored = amount;
            saveManager.Save();
        }

        public void SetAllStored(PlayerSet playerSet, long amount)
        {
            foreach (var player in playerSet.AllPlayers)
            {
                SetStored(player, amount);
            }

            saveManager.Save();
        }

        public bool Draw(MGPlayer player, long amount)
        {
            if (amount == 0)
            {
                return true;
            }

            DualPrint($"{player.FullName} draws {amount.Formatted()} gil from bank.");
            if (amount > player.Bank.Stored)
            {
                Plugin.Log.Info($"Trying to draw more than there is stored. {player.FullName}: {amount.Formatted()}/{player.Bank.Stored.Formatted()}");
                return false;
            }

            player.Bank.Stored -= amount;
            player.Bank.InUse += amount;

            saveManager.Save();
            return true;
        }

        public void StoreAll(MGPlayer player)
        {
            if (player.Bank.InUse == 0)
            {
                Plugin.Log.Info($"Store from {player.FullName}'s InUse top Stored skipped. Reason: 0 gil");
                return;
            }
            DualPrint($"{player.Bank.InUse.Formatted()} gil moved to bank for {player.FullName}");
            player.Bank.Stored += player.Bank.InUse;
            player.Bank.InUse = 0;

            saveManager.Save();
        }

        public void TransferInUse(MGPlayer source, MGPlayer destination)
        {
            if (source.Bank.InUse == 0)
            {
                Plugin.Log.Info($"Transfer from {source.FullName} to {destination.FullName} skipped. Reason: 0 gil");
                return;
            }
            DualPrint($"{source.Bank.InUse.Formatted()} gil moved from {source.FullName.GetFirstName()} to {destination.FullName.GetFirstName()}");
            destination.Bank.InUse += source.Bank.InUse;
            source.Bank.InUse = 0;

            saveManager.Save();
        }

        public void SetInUse(MGPlayer player, int amount)
        {
            player.Bank.InUse = amount;

            saveManager.Save();
        }

        private void DualPrint(string msg)
        {
            //chatGui.Print(msg);
            chatOutput.WriteChat(msg);
            Plugin.Log.Info(msg);
        }
    }
}
