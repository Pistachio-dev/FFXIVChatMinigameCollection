using Model.Banking;
using Model.PlayerManagement;
using PersistentModel.Model.PlayerManagement;


namespace PersistentModel.Repository.Interface
{
    public interface IPlayerRepository
    {
        public PlayerOOGData? GetPlayerWithCashRecord(string playerFullName);
        public bool CreatePlayer(PlayerOOGData playerData);
        public bool UpdateAlias(string name, string world, PlayerIdentifier newAlias);
        public bool UpdateCashRecord(PlayerOOGData player, GilTransaction newTransaction);
    }
}
