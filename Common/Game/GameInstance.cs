using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Model.PlayerManagement;

namespace CommonServices.Game
{
    public class GameInstance : IGameHost
    {
        public SessionPlayers Players { get; } = new();
    }
}
