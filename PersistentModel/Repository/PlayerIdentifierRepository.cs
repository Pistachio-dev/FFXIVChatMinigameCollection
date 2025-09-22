using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Repository
{
    public class PlayerIdentifierRepository : Repository<PlayerIdentifierEntity>, IPlayerIdentifierRepository
    {
        public PlayerIdentifierRepository(MinigameCollectionDbContext minigameCollectionDbContext) : base(minigameCollectionDbContext)
        {
        }
    }
}
