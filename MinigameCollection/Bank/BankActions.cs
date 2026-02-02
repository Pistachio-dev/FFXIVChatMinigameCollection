using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public void SetStored(MGPlayer player, long amount)
        {
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
            if (amount > player.Bank.Stored)
            {
                Plugin.Log.Info($"Trying to draw more than there is stored. {player.FullName}: {amount}/{player.Bank.Stored}");
                return false;
            }

            player.Bank.Stored -= amount;
            player.Bank.InUse += amount;

            return true;
        }

        public void StoreAll(MGPlayer player)
        {
            player.Bank.Stored += player.Bank.InUse;
            player.Bank.InUse = 0;
        }

        public void TransferInUse(MGPlayer source, MGPlayer destination)
        {
            destination.Bank.InUse += source.Bank.InUse;
            source.Bank.InUse = 0;
        }
    }
    
}
