using DalamudBasics.Configuration;
using MinigameCollection.Common.Banking;
using MinigameCollection.Common.Banking.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinigameCollection.Common.GameBoardCommon
{
    public class GameBoardBase
    {
        public GameBoardBase(IConfigurationService<Configuration> configurationService, PlayersInSessionManager playerManager)
        {
            this.config = configurationService.GetConfiguration();
            fakeBank = new FakeGilBank();
            realBank = new RealGilBank();
            this.PlayerManager = playerManager;
        }

        public bool UsingRealGil => config.UsingRealGil;

        protected Configuration config { get; }
        public IFakeGilBank fakeBank { get; }
        public IRealGilBank realBank { get; }

        public PlayersInSessionManager PlayerManager { get; }

        public List<PlayerInSession> Players => PlayerManager.InGame;
        
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
            var player = Players.FirstOrDefault(p => p.PlayerOOGData.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
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
