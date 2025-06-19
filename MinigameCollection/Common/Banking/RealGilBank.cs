using MinigameCollection.Common.Banking.Interface;
using System;

namespace MinigameCollection.Common.Banking
{
    public class RealGilBank : IRealGilBank
    {
        public long GetPlayerFunds(string playerName, string playerWorld)
        {
            throw new NotImplementedException();
        }

        public long ManuallySetFunds(string playerName, string playerWorld, long newFunds)
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
    }
}
