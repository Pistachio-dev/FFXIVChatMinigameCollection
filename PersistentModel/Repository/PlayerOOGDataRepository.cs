using Microsoft.EntityFrameworkCore;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Generic;
using PersistentModel.Repository.Interface;

namespace PersistentModel.Repository
{
    public class PlayerOOGDataRepository : Repository<PlayerOOGDataEntity>, IPlayerOOGDataRepository
    {
        public PlayerOOGDataRepository(MinigameCollectionDbContext minigameCollectionDbContext) : base(minigameCollectionDbContext)
        {
        }

        public PlayerOOGDataEntity? GetPlayerOrDefault(string name, string world)
        {
            return _ctx.PlayerOOGData.Include(r => r.CashRecord).ThenInclude(cashRecord => cashRecord.History).FirstOrDefault(p => p.Name == name && p.World == world);
        }

        public void AddPlayer(PlayerOOGDataEntity player)
        {
            Add(player);
        }

        public void RemovePlayer(PlayerOOGDataEntity player)
        {
            Delete(player);
        }
    }
}
