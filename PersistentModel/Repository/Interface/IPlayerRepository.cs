using PersistentModel.Model.PlayerManagement;


namespace PersistentModel.Repository.Interface
{
    public interface IPlayerRepository
    {
        public bool UpdatePlayer(PlayerOOGDataEntity playerData)
        {
            return true;
        }
    }
}
