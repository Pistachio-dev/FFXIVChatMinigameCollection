using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersistentModel.Repository;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<MinigameCollectionDbContext, MinigameCollectionDbContext>(options =>
                options.UseSqlite(MinigameCollectionDbContextFactory.GetConnectionString()));
            serviceCollection.AddTransient(typeof(IPlayerRepository), typeof(PlayerRepository));

            return serviceCollection;
        }

        public static void InitializeDatabaseIfNeeded(this IServiceProvider sp)
        {
            sp.GetRequiredService<MinigameCollectionDbContext>().Database.Migrate();
        }
    }
}
