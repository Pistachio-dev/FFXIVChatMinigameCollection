using Model.Banking.Transactions;
using Model.PlayerManagement;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Model.Banking
{
    public class GilTransactionEntity
    {
        [Key]
        public uint Id { get; }

        public uint HostPlayerId { get; }

        [Required]
        public PlayerOOGDataEntity HostPlayer { get; set; }

        public uint PatronPlayerId { get; set; }

        [Required]
        public PlayerOOGDataEntity PatronPlayer { get; set; }
        
        [Required]
        public bool IsRealGil { get; set; }

        [Required]
        public TransactionType Cause { get; set; }

        // Positive quantities = adding to the account
        [Required]
        public long InUseDiff { get; set; }

        [Required]
        public long StoredDiff { get; set; }

        [Required]
        public DateTime WhenUtc { get; set; }
    }
}
