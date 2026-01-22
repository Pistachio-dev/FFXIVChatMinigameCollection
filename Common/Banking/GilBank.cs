using Common.Banking.Interface;
using CommonServices.Banking.Interface;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Logging;
using MinigameCollection.Common.GameBoardCommon;
using Model.Banking;
using Model.Banking.Transactions;
using Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonServices.Banking
{
    public abstract class GilBank : IGilBank
    {
        private readonly ILogService log;
        private readonly IChatGui chatGui;
        private readonly ISessionPlayerManager playerManager;
        private readonly IPlayerRepository playerRepo;

        public GilBank(
                            ILogService logService,
                            IChatGui chatGui,
                            ISessionPlayerManager playerManager,
                            IPlayerRepository playerRepo)
        {
            this.log = logService;
            this.chatGui = chatGui;
            this.playerManager = playerManager;
            this.playerRepo = playerRepo;
        }

        protected abstract long GetInUseProperty(PlayerCashRecord record);

        protected abstract long GetStoredProperty(PlayerCashRecord record);

        protected abstract void SetInUseProperty(PlayerCashRecord record, long value);

        protected abstract void SetStoredProperty(PlayerCashRecord record, long value);
        protected abstract bool IsRealGil();

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null) return 0;

            return GetInUseProperty(stored.CashRecord);
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
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

            var transaction = GilTransaction.FromIntoPlay(host.PlayerOOGData, stored, amount, IsRealGil());
            playerRepo.UpdateCashRecord(stored, transaction);

            return true;
        }

        public bool StoreFunds(string fullPlayerName, long amount)
        {
            (var stored, var host, var result) = GetPlayers(fullPlayerName);
            if (result == false) return false;

            SetInUseProperty(stored.CashRecord, GetInUseProperty(stored.CashRecord) - amount);
            SetStoredProperty(stored.CashRecord, GetStoredProperty(stored.CashRecord) + amount);

            var transaction = GilTransaction.FromIntoPlay(host.PlayerOOGData, stored, amount, IsRealGil());
            playerRepo.UpdateCashRecord(stored, transaction);

            return true;
        }

        public bool ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            (var storedPlayer, var host, var result) = GetPlayers(fullPlayerName);
            if (result == false) return false;

            var transaction = GilTransaction.FromManualSet(host.PlayerOOGData, storedPlayer, newFunds - storedPlayer.CashRecord.StoredFake, IsRealGil());

            SetStoredProperty(storedPlayer.CashRecord, newFunds);

            playerRepo.UpdateCashRecord(storedPlayer, transaction);

            return true;
        }

        // Meant to be used by the game
        public bool ChangeInUseFunds(string fullPlayerName, long newAmount)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null)
            {
                return false;
            }

            var host = playerManager.GetOrAddHostPlayer();
            if (host == null) throw new Exception("Could not retrieve host player");
            var transaction = GilTransaction.FromChangeInGame(host.PlayerOOGData, stored, newAmount - stored.CashRecord.InUseFake, false);

            SetInUseProperty(stored.CashRecord, newAmount);

            playerRepo.UpdateCashRecord(stored, transaction);

            return true;
        }

        public abstract void StartBuyIn(string playerName, string playerWorld);
        public abstract void StartCashOut(string playerName, string playerWorld);

        private (PlayerOOGData? storedPlayer, PlayerInSession? host, bool result) GetPlayers(string fullPlayerName)
        {            
            var storedPlayer = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (storedPlayer == null)
            {
                throw new Exception($"Could not get player {fullPlayerName}'s cash record");
            }

            var host = playerManager.GetOrAddHostPlayer();
            if (host == null) {
                throw new Exception("Could not retrieve host player");
            }

            return (storedPlayer, host, true);
        }

        public bool SetStoredFunds(string fullPlayerName, long newFunds)
        {
            throw new NotImplementedException();
        }

        public long StoreAllGilInUse(string fullPlayerName)
        {
            throw new NotImplementedException();
        }
    }
}
