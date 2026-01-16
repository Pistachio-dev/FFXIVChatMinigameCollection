using MinigameCollection.Common.GameBoardCommon;

namespace CommonServices.PlayerManagement.Interface
{
    public interface ISessionPlayerManager
    {
        PlayerInSession? AddPlayer(string fullName, bool asSpectator = false);
        PlayerInSession? AddTargetPlayer();
        PlayerInSession? GetDealer();
        PlayerInSession? GetOrAddHostPlayer();
        PlayerInSession? GetPlayer(string fullName);
        bool IsPlayerInSession(string name, string world);
        bool MakePlayerActive(string fullName);
        bool MakePlayerSpectator(string fullName);
        void RemovePlayer(string fullName);
        bool TogglePlayerAsAFK(string fullName);
    }
}
