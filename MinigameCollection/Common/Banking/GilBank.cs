using MinigameCollection.Common.Banking.Interface;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.Banking
{
    public abstract class GilBank : IGilBank
    {
        public abstract long GetPlayerFunds(string playerName, string playerWorld);
        public abstract long ManuallySetFunds(string playerName, string playerWorld, long newFunds);


        //protected PlayerOOGData CreateFirstPlayerTransaction(string playerName, string playerWorld)
        //{

        //}
    }
}
