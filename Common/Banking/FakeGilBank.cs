using Common.Banking.Interface;
using CommonServices.PlayerManagement.Interface;
using DalamudBasics.Logging;
using Model.Banking;
using System;


namespace Common.Banking
{
    internal class FakeGilBank : IGilBank
    {
        private readonly ISessionPlayerManager playerManager;
        private readonly ILogService log;

        public FakeGilBank(
                            ISessionPlayerManager playerManager,
                            ILogService logService)
        {
            this.playerManager = playerManager;
            this.log = logService;
        }

        public bool DrawFromStored(string fullPlayerName, long amount)
        {
            throw new NotImplementedException();
            //var player = playerManager.GetPlayer(fullPlayerName);
            //if (player == null)
            //{
            //    logService.Warning($"Could not draw {amount} from {fullPlayerName}. Player not found.");
            //    return false;
            //}

            //if (player.CashRecord == null)
            //{
            //    player.CashRecord = new PlayerCashRecord();
            //    cashRecordRepository.Add(player.CashRecord);
            //}

            //if (player.CashRecord.StoredFake < amount)
            //{
            //    logService.Warning($"Could not draw {amount} from {fullPlayerName}. They only have {player.CashRecord.StoredFake}.");
            //    return false;
            //}

            //var dealer = playerManager.GetDealer();
            //var transaction = new GilTransaction(dealer, player, false, amount);
            //transactionRepo.Add(transaction);

            //return true;
        }

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            throw new NotImplementedException();
            //var player = playerManager.GetPlayer(fullPlayerName);
            //if (player == null)
            //{
            //    log.Info($"Attempted to get funds in use from {fullPlayerName} but the player does not exist");
            //    return 0;
            //}

            //return player.CashRecord.InUseFake;
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            throw new NotImplementedException();
            //var player = playerManager.GetPlayer(fullPlayerName);
            //if (player == null)
            //{
            //    log.Info($"Attempted to get funds stored from {fullPlayerName} but the player does not exist");
            //    return 0;
            //}

            //return player.CashRecord.StoredFake;
        }

        public long ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            throw new NotImplementedException();
            //var player = playerManager.GetPlayer(fullPlayerName);
            //if (player == null)
            //{
            //    log.Info($"Attempted to set stored fake funds from {fullPlayerName} but the player does not exist");
            //    return 0;
            //}

            //var transaction = new GilTransaction(playerManager.GetDealer(), player, false, newFunds);
            //player.CashRecord.AddTransaction(transaction);

            //return newFunds;
        }

        public long SetInUseFunds(string fullPlayerName, long amount)
        {
            throw new NotImplementedException();
            //var player = playerManager.GetPlayer(fullPlayerName);
            //if (player == null)
            //{
            //    log.Info($"Attempted to set in use fake funds from {fullPlayerName} but the player does not exist");
            //    return 0;
            //}

            //var transaction = new GilTransaction(playerManager.GetDealer(), player, false, amount);

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
