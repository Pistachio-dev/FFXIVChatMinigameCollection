using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Layer;
using Model.Banking.Transactions;
using System;

namespace CommonServices.Banking.Interface
{
    public interface IGilBank
    {
        public long GetPlayerInUseFunds(string fullPlayerName);

        public long GetPlayerStoredFunds(string fullPlayerName);

        public bool DrawFromStored(string fullPlayerName, long amount, bool allowDebt);

        public bool StoreFunds(string fullPlayerName, long amount);

        public bool ManuallySetStoredFunds(string fullPlayerName, long newFunds);

        // Meant to be used by the game
        public bool ChangeInUseFunds(string fullPlayerName, long newAmount);

        public abstract void StartBuyIn(string playerName, string playerWorld);

        public abstract void StartCashOut(string playerName, string playerWorld);
    }
}
