using Common.Banking.Interface;
using Common.PlayerManagement.Interface;
using DalamudBasics.Logging;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using Serilog;
using System;
using System.Runtime.CompilerServices;


namespace Common.Banking
{
    internal class FakeGilBank : IGilBank
    {
        private readonly IGilTransactionRepository transactionRepo;
        private readonly IPlayerCashRecordRepository cashRecordRepository;
        private readonly IPlayerManager playerManager;
        private readonly ILogService logService;

        public FakeGilBank(IGilTransactionRepository transactionRepo,
                            IPlayerCashRecordRepository cashRecordRepository,
                            IPlayerManager playerManager,
                            ILogService logService)
        {
            this.transactionRepo = transactionRepo;
            this.cashRecordRepository = cashRecordRepository;
            this.playerManager = playerManager;
            this.logService = logService;
        }

        public bool DrawFromStored(string fullPlayerName, long amount)
        {
            var player = playerManager.GetPlayer(fullPlayerName);
            if (player == null)
            {
                logService.Warning($"Could not draw {amount} from {fullPlayerName}. Player not found.");
                return false;
            }

            if (player.CashRecord == null)
            {
                player.CashRecord = new PlayerCashRecord();
                cashRecordRepository.Add(player.CashRecord);
            }

            if (player.CashRecord.StoredFake < amount)
            {
                logService.Warning($"Could not draw {amount} from {fullPlayerName}. They only have {player.CashRecord.StoredFake}.");
                return false;
            }

            var dealer = playerManager.GetDealer();
            var transaction = new GilTransaction(dealer, player, false, amount);
            transactionRepo.Add(transaction);

            return true;
        }

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            var player = playerManager.GetPlayer(fullPlayerName);
            if (player == null)
            {
                logService.Info($"Attempted to get funds in use from {fullPlayerName} but the player does not exist");
                return 0;
            }

            return player.CashRecord.InUseFake;
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            var player = playerManager.GetPlayer(fullPlayerName);
            if (player == null)
            {
                logService.Info($"Attempted to get funds stored from {fullPlayerName} but the player does not exist");
                return 0;
            }

            return player.CashRecord.StoredFake;
        }

        public long ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            var player = playerManager.GetPlayer(fullPlayerName);
            if (player == null)
            {
                logService.Info($"Attempted to set stored fake funds from {fullPlayerName} but the player does not exist");
                return 0;
            }
            
            var transaction = new GilTransaction(playerManager.GetDealer(), player, false, newFunds);
            player.CashRecord.AddTransaction(transaction);

            return newFunds;
        }

        public long SetInUseFunds(string fullPlayerName, long amount)
        {
            var player = playerManager.GetPlayer(fullPlayerName);
            if (player == null)
            {
                logService.Info($"Attempted to set in use fake funds from {fullPlayerName} but the player does not exist");
                return 0;
            }

            var transaction = new GilTransaction(playerManager.GetDealer(), player, false, amount);

            return amount;
        }

        public void StartBuyIn(string playerName, string playerWorld)
        {
            throw new NotImplementedException();
        }

        public void StartCashOut(string playerName, string playerWorld)
        {
            throw new NotImplementedException();
        }

        public long StoreAllGilInUse(string fullPlayerName)
        {
            throw new NotImplementedException();
        }
    }
}
