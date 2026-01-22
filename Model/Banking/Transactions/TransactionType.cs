using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Banking.Transactions
{
    public enum TransactionType
    {
        Game,
        HouseCut,
        CashIn,
        CashOut,
        ManuallySetStored,
        Play, // stored->inUse
        Bank, // inUse->Stored
    }
}
