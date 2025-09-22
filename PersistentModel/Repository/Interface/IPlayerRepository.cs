using PersistentModel.Model.PlayerManagement;


namespace PersistentModel.Repository.Interface
{
    public interface IPlayerRepository
    {
        public bool UpdatePlayer(PlayerOOGData playerData)
        {
            return true;
        }
    }
}
