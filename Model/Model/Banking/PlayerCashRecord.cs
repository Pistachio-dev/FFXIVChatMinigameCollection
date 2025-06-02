using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Model.Banking
{
    public class PlayerCashRecord
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public PlayerOOGData Player { get; set; }

        [Required]
        public long RealGilBalance { get; set; }

        [Required]
        public long FakeGilBalance { get; set; }

        [Required]
        public List<GilTransaction> History { get; set; } = new();

        public PlayerCashRecord(PlayerOOGData player)
        {
            Player = player;
            RealGilBalance = 0;
            FakeGilBalance = 0;
            History = new();
        }
    }
}
