using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.PlayerManagement.Data
{
    public class PlayerIdentifier
    {
        public string Name  { get; set; }
        public string World { get; set; }
        public DateTime DateMetUtc { get; set; }

        public PlayerIdentifier(string name, string world)
        {
            Name = name;
            World = world;
            DateMetUtc = DateTime.UtcNow;
        }
    }
}
