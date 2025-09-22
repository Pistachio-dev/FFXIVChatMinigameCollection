using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Common.UICommon;

namespace MinigameCollection.Common
{
    public static class ServiceCollectionExtensiosn
    {
        public static IServiceCollection AddGamesBase(this IServiceCollection sc)
        {

            sc.AddSingleton<GameUITabBase>();
            sc.AddSingleton<GameModeManager>();

            return sc;
        }
    }
}
