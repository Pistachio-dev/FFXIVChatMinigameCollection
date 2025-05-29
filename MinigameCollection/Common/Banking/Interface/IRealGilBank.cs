using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.Banking.Interface
{
    public interface IRealGilBank : IGilBank
    {
        public void StartCashOut(string playerName, string playerWorld);
        public void StartBuyIn(string playerName, string playerWorld);
    }
}
