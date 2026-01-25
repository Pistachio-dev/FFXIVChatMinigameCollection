using MinigameCollection.Common.GameBoardCommon;

namespace CommonServices.PlayerManagement.Interface
{
    public interface ISessionPlayerManager<T> where T : IGameSpecificPlayerData
    {
        PlayerInSession<T>? AddPlayer(string fullName, bool asSpectator = false);
        PlayerInSession<T>? AddTargetPlayer();
        PlayerInSession<T>? GetDealer();
        PlayerInSession<T> GetOrAddHostPlayer();
        PlayerInSession<T>? GetPlayer(string fullName);
        bool IsPlayerInSession(string name, string world);
        bool MakePlayerActive(string fullName);
        bool MakePlayerSpectator(string fullName);
        void RemovePlayer(string fullName);
        bool TogglePlayerAsAFK(string fullName);
    }
}
