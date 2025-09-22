using Microsoft.EntityFrameworkCore;
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

        public PlayerOOGData? GetPlayerOrDefault(string name, string world)
        {
            return _ctx.PlayerOOGData.Include(r => r.CashRecord).ThenInclude(cashRecord => cashRecord.History).FirstOrDefault(p => p.Name == name && p.World == world);
        }

        public void AddPlayer(PlayerOOGData player)
        {
            Add(player);
        }

        public void RemovePlayer(PlayerOOGData player)
        {
            Delete(player);
        }
    }
}
