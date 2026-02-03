using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using Model.Base;

namespace MinigameCollection.Bank
{
    public class BankActions
    {
        private readonly IChatOutput chatOutput;
        private readonly IClientChatGui chatGui;

        public BankActions(IChatOutput chatOutput, IClientChatGui chatGui)
        {
            this.chatOutput = chatOutput;
            this.chatGui = chatGui;
        }

        // Negative values to substract
        // Yes this lets you get into debt
        public void AddStored(MGPlayer player, long amount)
        {
            player.Bank.Stored += amount;
            DualPrint($"{amount.Formatted()} added to {player.FullName}'s bank.");
        }

        public void SetStored(MGPlayer player, long amount)
        {
            DualPrint($"{player.FullName} bank is set to {amount.Formatted()} gil");
            player.Bank.Stored = amount;
        }

        public void SetAllStored(PlayerSet playerSet, long amount)
        {
            foreach (var player in playerSet.Players)
            {
                SetStored(player, amount);
            }
        }

        public bool Draw(MGPlayer player, long amount)
        {
            DualPrint($"{player.FullName} draws {amount.Formatted()} gil from bank.");
            if (amount > player.Bank.Stored)
            {
                Plugin.Log.Info($"Trying to draw more than there is stored. {player.FullName}: {amount.Formatted()}/{player.Bank.Stored.Formatted()}");
                return false;
            }

            player.Bank.Stored -= amount;
            player.Bank.InUse += amount;

            return true;
        }

        public void StoreAll(MGPlayer player)
        {
            DualPrint($"{player.Bank.InUse} gil moved to bank for {player.FullName}");
            player.Bank.Stored += player.Bank.InUse;
            player.Bank.InUse = 0;
        }

        public void TransferInUse(MGPlayer source, MGPlayer destination)
        {
            DualPrint($"{source.Bank.InUse} gil moved from {source.FullName} to {destination.FullName}");

            destination.Bank.InUse += source.Bank.InUse;
            source.Bank.InUse = 0;
        }

        private void DualPrint(string msg)
        {
            chatGui.Print(msg);
            chatOutput.WriteChat(msg);
            Plugin.Log.Info(msg);
        }
    }
}
