using Common.Banking.Interface;
using CommonServices.Banking.Enum;
using PersistentModel.Repository.Interface;
using System;

namespace Common.Banking
{
    internal class RealGilBank : IGilBank
    {
        private IPlayerRepository playerRepo;

        public RealGilBank(IPlayerRepository playerRepository)
        {
            playerRepo = playerRepository;
        }

        public BankType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long DrawFromStored(string fullPlayerName, long amount)
        {
            throw new NotImplementedException();
            var player = playerRepo.GetPlayerWithCashRecord(fullPlayerName);
            if (player != null)
            {

            }
        }

        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            throw new NotImplementedException();
        }

        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            throw new NotImplementedException();
        }

        public long ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            throw new NotImplementedException();
        }

        public long SetInUseFunds(string fullPlayerName, long amount)
        {
            throw new NotImplementedException();
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

        bool IGilBank.DrawFromStored(string fullPlayerName, long amount)
        {
            throw new NotImplementedException();
        }
    }
}
