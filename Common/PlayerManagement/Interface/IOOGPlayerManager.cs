using Model.PlayerManagement;

namespace CommonServices.PlayerManagement.Interface
{
    public interface IOOGPlayerManager
    {
        PlayerOOGData? GetPlayer(string fullName);

        bool UpdateIdentity(string fullName, string newName, string newWorld);
    }
}
