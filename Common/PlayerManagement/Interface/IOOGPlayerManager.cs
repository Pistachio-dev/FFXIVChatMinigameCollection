using Model.Banking.Transactions;
using Model.PlayerManagement;

namespace CommonServices.PlayerManagement.Interface
{
    public interface IOOGPlayerManager
    {
        PlayerOOGData? CreatePlayer(string name, string world);
        PlayerOOGData? GetOrCreateHostPlayer();
        PlayerOOGData? GetPlayerWithCashRecord(string fullName);
        bool UpdateCashRecord(PlayerOOGData updatedPlayer, GilTransaction newTransaction);
        bool UpdateIdentity(string fullName, string newName, string newWorld);
    }
}
