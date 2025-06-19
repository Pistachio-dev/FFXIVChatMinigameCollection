using Microsoft.Extensions.DependencyInjection;

namespace MinigameCollection.Games.NoGame
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoGame(this IServiceCollection sc)
        {
            sc.AddSingleton<NoGameAction>();
            sc.AddSingleton<NoGameBoard>();
            sc.AddSingleton<NoGameUITab>();

            return sc;
        }
    }
}
