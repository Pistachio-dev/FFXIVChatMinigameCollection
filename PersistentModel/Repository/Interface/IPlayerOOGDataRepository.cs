using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Generic;

namespace PersistentModel.Repository.Interface
{
    public interface IPlayerOOGDataRepository : IRepository<PlayerOOGDataEntity>
    {
        void AddPlayer(PlayerOOGDataEntity player);
        void RemovePlayer(PlayerOOGDataEntity player);
        PlayerOOGDataEntity? GetPlayerOrDefault(string name, string world);
    }
}
