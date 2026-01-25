using CommonServices.Banking.Interface;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Logging;
using Model.Banking;
using Model.Banking.Transactions;
using Model.PlayerManagement;
using System;

namespace CommonServices.Banking
{
    public abstract class GilBank : IGilBank
    {
        private readonly ILogService log;
        private readonly IChatGui chatGui;
        private readonly IOOGPlayerManager oogPlayerMng;

        public GilBank(
                            ILogService logService,
                            IChatGui chatGui,
                            IOOGPlayerManager oogPlayerMng)
        {
            this.log = logService;
            this.chatGui = chatGui;
            this.oogPlayerMng = oogPlayerMng;
        }

        protected abstract long GetInUseProperty(PlayerCashRecord record);

        protected abstract long GetStoredProperty(PlayerCashRecord record);

        protected abstract void SetInUseProperty(PlayerCashRecord record, long value);

        protected abstract void SetStoredProperty(PlayerCashRecord record, long value);
        protected abstract bool IsRealGil();

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            var stored = oogPlayerMng.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null) return 0;

            return GetInUseProperty(stored.CashRecord);
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            var stored = oogPlayerMng.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null) return 0;

           return GetStoredProperty(stored.CashRecord);
        }

        public bool DrawFromStored(string fullPlayerName, long amount, bool allowDebt)
        {
            (var stored, var host, var result) = GetPlayers(fullPlayerName);
            if (result == false) return false;

            if (amount > stored.CashRecord.StoredFake && !allowDebt)
            {
                chatGui.PrintError($"Player {fullPlayerName} can't draw {amount} gil: they don't have enough");
                return false;
            }

            SetInUseProperty(stored.CashRecord, GetInUseProperty(stored.CashRecord) + amount);
            SetStoredProperty(stored.CashRecord, GetStoredProperty(stored.CashRecord) - amount);

            var transaction = GilTransaction.FromStoredToInUse(host, stored, amount, IsRealGil());
            oogPlayerMng.UpdateCashRecord(stored, transaction);

            return true;
        }

        public bool StoreFunds(string fullPlayerName, long amount)
        {
            (var stored, var host, var result) = GetPlayers(fullPlayerName);
            if (result == false) return false;

            SetInUseProperty(stored.CashRecord, GetInUseProperty(stored.CashRecord) - amount);
            SetStoredProperty(stored.CashRecord, GetStoredProperty(stored.CashRecord) + amount);

            var transaction = GilTransaction.FromStoredToInUse(host, stored, amount, IsRealGil());
            oogPlayerMng.UpdateCashRecord(stored, transaction);

            return true;
        }

        public bool ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            (var storedPlayer, var host, var result) = GetPlayers(fullPlayerName);
            if (result == false) return false;

            var transaction = GilTransaction.FromManuallySettingStored(host, storedPlayer, newFunds - storedPlayer.CashRecord.StoredFake, IsRealGil());

            SetStoredProperty(storedPlayer.CashRecord, newFunds);

            oogPlayerMng.UpdateCashRecord(storedPlayer, transaction);

            return true;
        }

        // Meant to be used by the game
        public bool ChangeInUseFunds(string fullPlayerName, long newAmount)
        {
            var stored = oogPlayerMng.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null)
            {
                return false;
            }

            var host = oogPlayerMng.GetOrCreateHostPlayer();
            if (host == null) throw new Exception("Could not retrieve host player");
            var transaction = GilTransaction.FromManuallySettingInUse(host, stored, newAmount - stored.CashRecord.InUseFake, false);

            SetInUseProperty(stored.CashRecord, newAmount);

            oogPlayerMng.UpdateCashRecord(stored, transaction);

            return true;
        }

        public abstract void StartBuyIn(string playerName, string playerWorld);
        public abstract void StartCashOut(string playerName, string playerWorld);

        private (PlayerOOGData? storedPlayer, PlayerOOGData? host, bool result) GetPlayers(string fullPlayerName)
        {            
            var storedPlayer = oogPlayerMng.GetPlayerWithCashRecord(fullPlayerName);
            if (storedPlayer == null)
            {
                throw new Exception($"Could not get player {fullPlayerName}'s cash record");
            }

            var host = oogPlayerMng.GetOrCreateHostPlayer();
            if (host == null) {
                throw new Exception("Could not retrieve host player");
            }

            return (storedPlayer, host, true);
        }
    }
}
