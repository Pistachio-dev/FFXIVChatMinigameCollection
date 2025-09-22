using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Model.PlayerManagement
{
    [Index(nameof(Name))]
    public class PlayerIdentifier
    {
        [Key]
        public uint Id { get; set; }       

        [Required]
        public string Name  { get; set; }
        
        [Required]
        public string World { get; set; }
        
        [Required]
        public DateTime DateMetUtc { get; set; }

        public uint PlayerOOGDataId { get; set; }

        public PlayerOOGData PlayerOOGData { get; set; }

        public PlayerIdentifier(string name, string world)
        {
            Name = name;
            World = world;
            DateMetUtc = DateTime.UtcNow;
        }
    }
}
