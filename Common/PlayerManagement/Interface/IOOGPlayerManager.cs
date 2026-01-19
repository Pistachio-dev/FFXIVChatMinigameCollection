using Model.PlayerManagement;

namespace CommonServices.PlayerManagement.Interface
{
    public interface IOOGPlayerManager
    {
        PlayerOOGData? CreatePlayer(string name, string world);
        PlayerOOGData? GetPlayerWithCashRecord(string fullName);

        bool UpdateIdentity(string fullName, string newName, string newWorld);
    }
}
