using Common.Banking.Interface;
using CommonServices.Banking.Enum;
using CommonServices.Banking.Interface;
using Model.Banking.Transactions;
using System;

namespace Common.Banking
{
    internal class GilBanksContainer : IGilBanksContainer
    {
        public GilBanksContainer(FakeGilBank fakeGilBank, RealGilBank realGilBank)
        {
            FakeGilBank = fakeGilBank;
            RealGilBank = realGilBank;
        }
        
        protected BankType _bankType = BankType.RealGil;

        public BankType Type { get => _bankType; set => _bankType = value; }

        private FakeGilBank FakeGilBank { get; set; }
        private RealGilBank RealGilBank { get; set; }

        /// <inheritdoc/>
        public long GetPlayerInUseFunds(string fullPlayerName)
        {
            return GetActiveBank().GetPlayerInUseFunds(fullPlayerName);
        }

        /// <inheritdoc/>
        public long GetPlayerStoredFunds(string fullPlayerName)
        {
            return GetActiveBank().GetPlayerStoredFunds(fullPlayerName);
        }

        /// <inheritdoc/>
        public bool DrawFromStored(string fullPlayerName, long amount, bool allowDebt)
        {
            return GetActiveBank().DrawFromStored(fullPlayerName, amount, allowDebt);
        }

        public bool StoreFunds(string fullPlayerName, long amount)
        {
            return GetActiveBank().StoreFunds(fullPlayerName, amount);
        }

        /// <inheritdoc/>
        public bool ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            return GetActiveBank().ManuallySetStoredFunds(fullPlayerName, newFunds);
        }

        /// <inheritdoc/>
        public bool ChangeInUseFunds(string fullPlayerName, long amount)
        {
            return GetActiveBank().ChangeInUseFunds(fullPlayerName, amount);
        }

        public IGilBank GetActiveBank()
        {
            return BankType.FakeGil == Type ? FakeGilBank : RealGilBank;
        }

        public void StartCashOut(string playerName, string playerWorld)
        {
            if (Type == BankType.FakeGil)
            {
                return;
            }

            RealGilBank.StartCashOut(playerName, playerWorld);
        }

        public void StartBuyIn(string playerName, string playerWorld)
        {
            if (Type == BankType.FakeGil)
            {
                return;
            }

            RealGilBank.StartBuyIn(playerName, playerWorld);
        }

        public bool SetStoredFunds(string fullPlayerName, long newFunds)
        {
            throw new NotImplementedException();
        }
    }
}
