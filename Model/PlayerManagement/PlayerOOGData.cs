using Model.Banking;
using System.ComponentModel.DataAnnotations;

namespace Model.PlayerManagement
{
    // Data for a player unrelated to the current game
    public class PlayerOOGData
    {
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

        public string FullName  => $"{Name}@{World}";        

        public bool Is(string name, string world)
        {
            return Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && World.Equals(world, StringComparison.OrdinalIgnoreCase);
        }
    }
}
