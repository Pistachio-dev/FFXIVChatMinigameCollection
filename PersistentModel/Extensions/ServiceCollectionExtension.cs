using Microsoft.Extensions.DependencyInjection;
using PersistentModel.Repository;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<MinigameCollectionDbContext>();
            serviceCollection.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddTransient(typeof(IGilTransactionRepository), typeof(GilTransactionRepository));
            serviceCollection.AddTransient(typeof(IPlayerCashRecordRepository), typeof(PlayerCashRecordRepository));
            serviceCollection.AddTransient(typeof(IPlayerIdentifierRepository), typeof(PlayerIdentifierRepository));
            serviceCollection.AddTransient(typeof(IPlayerOOGDataRepository), typeof(PlayerOOGDataRepository));

            return serviceCollection;
        }
    }
}
