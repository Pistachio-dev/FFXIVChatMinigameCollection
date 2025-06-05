using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Repository
{
    public class PlayerOOGDataRepository : Repository<PlayerOOGData>, IPlayerOOGDataRepository
    {
        public PlayerOOGDataRepository(MinigameCollectionDbContext minigameCollectionDbContext) : base(minigameCollectionDbContext)
        {
        }

        public PlayerOOGData? TryGetPlayer(string name, string world)
        {
            throw new NotImplementedException();
        }
    }
}
