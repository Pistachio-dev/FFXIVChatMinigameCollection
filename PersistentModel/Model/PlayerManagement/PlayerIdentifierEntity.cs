using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PersistentModel.Model.PlayerManagement
{
    [Index(nameof(Name))]
    public class PlayerIdentifierEntity
    {
        [Key]
        public uint Id { get; set; }       

        [Required]
        public string Name  { get; set; }
        
        [Required]
        public string World { get; set; }
        
        [Required]
        public DateTime DateIdentityChanged { get; set; }

        public uint PlayerOOGDataId { get; set; }

        public PlayerOOGDataEntity PlayerOOGData { get; set; }
    }
}
