using Common.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Banking.Interface
{
    public interface IGilBanksContainer : IGilBank, ITradeEnabledBank
    {
        public BankType Type { get; set; }

        protected IGilBank GetActiveBank();
    }
}
