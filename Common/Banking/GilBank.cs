using Common.Banking.Interface;
using CommonServices.Banking.Enum;

namespace Common.Banking
{
    internal class GilBank : IGilBanksContainer
    {
        public GilBank(FakeGilBank fakeGilBank, RealGilBank realGilBank)
        {
            FakeGilBank = fakeGilBank;
            RealGilBank = realGilBank;
        }
        
        protected BankType _bankType = BankType.RealGil;

        public BankType Type { get => _bankType; set => _bankType = value; }

        private FakeGilBank FakeGilBank { get; set; }
        private RealGilBank RealGilBank { get; set; }

        /// <inheritdoc/>
        public bool DrawFromStored(string fullPlayerName, long amount)
        {
            return GetActiveBank().DrawFromStored(fullPlayerName, amount);
        }

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
        public long ManuallySetStoredFunds(string fullPlayerName, long newFunds)
        {
            return GetActiveBank().ManuallySetStoredFunds(fullPlayerName, newFunds);
        }

        /// <inheritdoc/>
        public long SetInUseFunds(string fullPlayerName, long amount)
        {
            return GetActiveBank().SetInUseFunds(fullPlayerName, amount);
        }

        /// <inheritdoc/>
        public long StoreAllGilInUse(string fullPlayerName)
        {
            return GetActiveBank().StoreAllGilInUse(fullPlayerName);
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
    }
}
