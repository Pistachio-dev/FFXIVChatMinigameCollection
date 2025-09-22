using MinigameCollection.Common.GameBoardCommon;
using PersistentModel.Model.PlayerManagement;
using System.Collections.Generic;

namespace Common.PlayerManagement.Interface
{
    public interface IPlayerManager
    {
        public List<PlayerInSession> InGame { get; }

        public PlayerOOGData GetDealer();

        public PlayerOOGData? GetPlayer(string fullName);
        public PlayerInSession? AddPlayer(string fullName);

        public PlayerInSession? AddTargetPlayer();

        public void RemovePlayer(string fullName);

        public bool IsPlayerInSession(string name, string world);

        public bool TogglePlayerAsAFK(string name, string world);
    }
}
