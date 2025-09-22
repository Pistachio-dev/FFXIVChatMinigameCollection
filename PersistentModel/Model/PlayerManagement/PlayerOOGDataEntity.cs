using Microsoft.EntityFrameworkCore;
using PersistentModel.Model.Banking;
using System.ComponentModel.DataAnnotations;

namespace PersistentModel.Model.PlayerManagement
{
    // Data for a player unrelated to the current game
    [Index(nameof(Name))]
    public class PlayerOOGDataEntity
    {
        [Key]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string World { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; }

        [Required]
        public List<PlayerIdentifierEntity> PreviousIdentities { get; set; } = new();

        [Required]
        public PlayerCashRecordEntity CashRecord { get; set; } = new();
    }
}
