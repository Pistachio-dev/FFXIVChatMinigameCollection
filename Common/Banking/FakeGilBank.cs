using Common.Banking.Interface;
using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Logging;
using Model.Banking;
using PersistentModel.Repository.Interface;
using System;


namespace Common.Banking
{
    internal class FakeGilBank : IGilBank
    {
        private readonly ILogService log;
        private readonly IChatGui chatGui;
        private readonly ISessionPlayerManager playerManager;
        private readonly IPlayerRepository playerRepo;

        public FakeGilBank(
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

        public bool DrawFromStored(string fullPlayerName, long amount, bool allowDebt)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null)
            {
                return false;
            }

            if (amount > stored.CashRecord.StoredFake && !allowDebt)
            {
                chatGui.PrintError($"Player {fullPlayerName} can't draw {amount} gil: they don't have enough");
                return false;
            }

            stored.CashRecord.StoredFake -= amount;
            stored.CashRecord.InUseFake += amount;

            var host = playerManager.GetOrAddHostPlayer() ?? throw new Exception("Host could not be retrieved");
            var transaction = GilTransaction.NewFakeGilTransaction(host.PlayerOOGData, stored, false, amount);
            playerRepo.UpdateCashRecord(stored, transaction);
            throw new NotImplementedException();
        }

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);

            return stored?.CashRecord.InUseFake ?? 0;
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);

            return stored?.CashRecord.StoredFake ?? 0;
        }

        public bool ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null)
            {
                return false;
            }

            var host = playerManager.GetOrAddHostPlayer();
            if (host == null) throw new Exception("Could not retrieve host player");
            var transaction = GilTransaction.NewFakeGilTransaction(host.PlayerOOGData, stored, false, newFunds -  stored.CashRecord.StoredFake);

            stored.CashRecord.StoredFake = newFunds;

            playerRepo.UpdateCashRecord(stored, transaction);

            return true;
        }

        // Meant to be used by the game
        public bool SetInUseFunds(string fullPlayerName, long amount)
        {
            var stored = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (stored == null)
            {
                return false;
            }

            var host = playerManager.GetOrAddHostPlayer();
            if (host == null) throw new Exception("Could not retrieve host player");
            var transaction = GilTransaction.NewFakeGilTransaction(host.PlayerOOGData, stored, false, amount - stored.CashRecord.InUseFake);

            stored.CashRecord.InUseFake = amount;

            playerRepo.UpdateCashRecord(stored, transaction);

            return true;
        }

        public void StartBuyIn(string playerName, string playerWorld)
        {
            chatGui.PrintError($"No buy ins with fake cash");
            log.Info($"Attempted buy in in fake cash mode");
        }

        public void StartCashOut(string playerName, string playerWorld)
        {
            chatGui.PrintError($"No cash outs with fake cash");
            log.Info($"Attempted cash out in fake cash mode");
        }

        public bool StoreAllGilInUse(string fullPlayerName)
        {
            return DrawFromStored() // You need to rethink what the transactions actuall mean. Can you transact between InUse and Stored?
        }
    }
}
