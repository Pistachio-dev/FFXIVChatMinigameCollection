using CommonServices.Game.Instance;
using Model.PlayerManagement;

namespace CommonServices.Game
{
    public class GameInstance : IGameInstance
    {
        public SessionPlayers Players { get; } = new();
    }
}
