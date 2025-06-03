using PersistentModel.Model.Banking;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Repository
{
    public class GilTransactionRepository : Repository<GilTransaction>, IGilTransactionRepository
    {
        public GilTransactionRepository(MinigameCollectionDbContext minigameCollectionDbContext) : base(minigameCollectionDbContext)
        {
        }
    }
}
