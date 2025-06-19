using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Common.GameActionsCommon;
using MinigameCollection.Common.GameBoardCommon;
using MinigameCollection.Common.UICommon;

namespace MinigameCollection.Common
{
    public static class ServiceCollectionExtensiosn
    {
        public static IServiceCollection AddGamesBase(this IServiceCollection sc)
        {
            sc.AddSingleton<PlayersInSessionManager>();
            sc.AddSingleton<GameActionsBase>();
            sc.AddSingleton<GameBoardBase>();
            sc.AddSingleton<GameUITabBase>();
            sc.AddSingleton<GameModeManager>();

            return sc;
        }
    }
}
