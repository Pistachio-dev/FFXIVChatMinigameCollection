using DalamudBasics.Configuration;
using MinigameCollection.Common.Banking.Interface;
using System;
using System.Linq;

namespace MinigameCollection.Common.Game
{
    public abstract class GameBoardBase
    {
        public GameBoardBase(IConfigurationService<Configuration> configurationService)
        {
            this.config = configurationService.GetConfiguration();
        }

        public abstract string GameMode { get; }
        public bool UsingRealGil => config.UsingRealGil;

        private Configuration config { get; }
        private readonly IFakeGilBank fakeBank;
        private readonly IRealGilBank realBank;
        private readonly PlayerBase[] players;
        
        public bool AddPlayer(string name, string world)
        {
            // If not on DB, create it.
            // Then add it to the list of players.

            return true;
        }

        public bool RemovePlayer(string name, string world)
        {
            // TODO

            return true;
        }

        public bool TogglePlayerAsAFK(string name, string world)
        {
            var player = players.FirstOrDefault(p => p.PlayerOOGData.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                && p.PlayerOOGData.World.Equals(world, StringComparison.CurrentCultureIgnoreCase));

            if (player == null)
            {
                return false;
            }

            player.IsAFK = !player.IsAFK;

            return true;
        }
    }
}
