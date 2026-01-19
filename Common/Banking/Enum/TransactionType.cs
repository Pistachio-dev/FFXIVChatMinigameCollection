using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Banking.Enum
{
    internal enum TransactionType
    {
        SetInUse,
        SetStored,
        MoveInUseToStored,
        MoveStoredToInUse        
    }
}
