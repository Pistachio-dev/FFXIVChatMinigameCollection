using Microsoft.EntityFrameworkCore;
using PersistentModel.Model.Banking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Model.PlayerManagement
{
    // Data for a player unrelated to the current game
    [Index(nameof(Name))]
    public class PlayerOOGData
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required]
        public string World { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }
        
        [Required]
        public List<PlayerIdentifier> PreviousIdentities { get; set; }
        
        [Required]
        public PlayerCashRecord CashRecord { get; set; }

        public PlayerOOGData(string name, string world)
        {
            Name = name;
            World = world;
            CreatedAtUtc = DateTime.UtcNow;
            PreviousIdentities = new();
            CashRecord = new PlayerCashRecord(this);
        }
    }
}
