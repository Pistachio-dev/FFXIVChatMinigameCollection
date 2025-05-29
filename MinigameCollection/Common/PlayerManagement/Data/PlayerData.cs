using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.PlayerManagement.Data
{
    public class PlayerData
    {
        public Guid Id { get; set; }
        private PlayerIdentifier PlayerIdentifier { get; set; }
        public string Name => PlayerIdentifier.Name;
        public string World => PlayerIdentifier.World;
        public List<PlayerIdentifier> PreviousIdentities { get; set; }

    }
}
