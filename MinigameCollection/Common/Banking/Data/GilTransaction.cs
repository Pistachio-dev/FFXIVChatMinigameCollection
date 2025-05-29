using MinigameCollection.Common.PlayerManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.Banking.Data
{
    public class GilTransaction
    {
        public Guid HostPlayerId { get; set; }
        public Guid PatronPlayerId { get; set; }
        public bool IsRealGil { get; set; }
        public bool IsHouseCut { get; set; }
        public long Amount { get; } // Positive amounts means the banked amount for the player increases
        public DateTime WhenUtc {  get; }

        public static GilTransaction NewFakeGilTransaction(PlayerData host, PlayerData patron, bool isHouseCut, long amount)
        {
            var newTransaction = new GilTransaction(host.Id, patron.Id, isHouseCut, amount);
            newTransaction.IsRealGil = false;

            return newTransaction;
        }

        public static GilTransaction NewRealGilTransaction(PlayerData host, PlayerData patron, bool isHouseCut, long amount)
        {
            var newTransaction = new GilTransaction(host.Id, patron.Id, isHouseCut, amount);
            newTransaction.IsRealGil = true;

            return newTransaction;
        }

        private GilTransaction(Guid hostPlayerId, Guid patronPlayerId, bool isHouseCut, long amount)
        {
            HostPlayerId = hostPlayerId;
            PatronPlayerId = patronPlayerId;
            IsHouseCut = isHouseCut;
            Amount = amount;
            WhenUtc = DateTime.UtcNow;
        }
    }
}
