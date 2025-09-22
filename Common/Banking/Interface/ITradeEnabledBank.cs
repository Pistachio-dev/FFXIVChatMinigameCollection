using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Banking.Interface
{
    public interface ITradeEnabledBank
    {
        // Start a cash out process, if possible
        public void StartCashOut(string playerName, string playerWorld);

        // Start a buy in process, if possible
        public void StartBuyIn(string playerName, string playerWorld);
    }
}
