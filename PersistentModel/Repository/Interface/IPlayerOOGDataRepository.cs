using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Generic;

namespace PersistentModel.Repository.Interface
{
    public interface IPlayerOOGDataRepository : IRepository<PlayerOOGData>
    {
        void AddPlayer(PlayerOOGData player);
        void RemovePlayer(PlayerOOGData player);
        PlayerOOGData? GetPlayerOrDefault(string name, string world);
    }
}
