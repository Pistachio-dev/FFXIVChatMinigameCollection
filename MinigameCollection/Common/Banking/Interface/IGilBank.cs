using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.Banking.Interface
{
    public interface IGilBank
    {
        long GetPlayerFunds(string playerName, string playerWorld);
        long ManuallySetFunds(string playerName, string playerWorld, long newFunds);
    }
}
