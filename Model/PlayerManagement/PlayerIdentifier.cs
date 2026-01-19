using System.ComponentModel.DataAnnotations;

namespace Model.PlayerManagement
{
    public class PlayerIdentifier
    {
        [Required]
        public string Name  { get; set; }
        
        [Required]
        public string World { get; set; }
        
        [Required]
        public DateTime DateIdentityChanged { get; set; }

        public PlayerIdentifier(string name, string world)
        {
            Name = name;
            World = world;
            DateIdentityChanged = DateTime.UtcNow;
        }

        internal PlayerIdentifier() { }
    }
}
