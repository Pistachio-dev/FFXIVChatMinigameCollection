using PersistentModel.Model.PlayerManagement;
using System;

namespace MinigameCollection.Common.GameBoardCommon
{
    public class PlayerInSession
    {
        public PlayerInSession(PlayerOOGData player)
        {
            PlayerOOGData = player;
        }

        public PlayerOOGData PlayerOOGData { get; set; }
        
        public bool IsAFK { get; set; } = false;

        public string Status { get; set; } = "Playing";

        public DateTime JoinedTimeUtc {  get; set; } = DateTime.UtcNow;

        public string FullName => PlayerOOGData.FullName;
        public bool Is(string name, string world)
        {
            return PlayerOOGData.Is(name, world);
        }
    }
}
