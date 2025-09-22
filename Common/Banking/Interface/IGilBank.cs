using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Banking.Interface
{
    public interface IGilBank
    {
        // Returns the TOTAL amount of funds
        public long GetPlayerTotalFunds(string fullPlayerName) => GetPlayerInUseFunds(fullPlayerName) + GetPlayerStoredFunds(fullPlayerName);
        
        // Returns the funds currently tied to a game
        public abstract long GetPlayerInUseFunds(string fullPlayerName);

        // Returns the funds currently not tied to a game
        public abstract long GetPlayerStoredFunds(string fullPlayerName);

        // Moves funds from Stored to InUse
        public abstract bool DrawFromStored(string fullPlayerName, long amount);

        // Merge InUse back into Stored. If you need to push a bet, just Store then Draw
        public abstract long StoreAllGilInUse(string fullPlayerName);

        // Manually sets the Stored gil amount
        public long ManuallySetStoredFunds(string fullPlayerName, long newFunds);

        // Change InUse funds. This is how you reflect wins or losses
        public long SetInUseFunds(string fullPlayerName, long amount);
    }
}
