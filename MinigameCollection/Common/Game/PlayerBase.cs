using PersistentModel.Model.PlayerManagement;

namespace MinigameCollection.Common.Game
{
    public class PlayerBase(PlayerOOGData playerOOGData)
    {
        public PlayerOOGData PlayerOOGData { get; set; } = playerOOGData;
        public bool IsAFK { get; set; } = false;
    }
}
