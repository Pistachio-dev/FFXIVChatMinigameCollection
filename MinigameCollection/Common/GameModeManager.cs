using DalamudBasics.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Common.UICommon;
using MinigameCollection.Games.NoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common
{
    internal class GameModeManager
    {
        Configuration config;
        private readonly IServiceProvider serviceProvider;
        private IConfigurationService<Configuration> configService;
        private Dictionary<GameSelected, GameUITab> loadedGames = new();

        public GameModeManager(IServiceProvider serviceProvider, IConfigurationService<Configuration> configurationService)
        {
            this.serviceProvider = serviceProvider;
            configService = configurationService;
            config = configurationService.GetConfiguration();
        }

        public GameUITab GetGame(GameSelected gameType)
        {
            if (!loadedGames.ContainsKey(gameType))
            {
                loadedGames[gameType] = InstanceGame(gameType);
            }

            return loadedGames[gameType];
        }

        private GameUITab InstanceGame(GameSelected gameType) {
            switch (gameType)
            {
                case GameSelected.None:
                    return serviceProvider.GetRequiredService<NoGameUITab>();
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
