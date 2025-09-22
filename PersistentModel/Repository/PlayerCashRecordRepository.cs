using PersistentModel.Model.Banking;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Repository
{
    public class PlayerCashRecordRepository : Repository<PlayerCashRecordEntity>, IPlayerCashRecordRepository
    {
        public PlayerCashRecordRepository(MinigameCollectionDbContext minigameCollectionDbContext) : base(minigameCollectionDbContext)
        {
        }
    }
}
