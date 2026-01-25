using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Model.PlayerManagement;

namespace CommonServices.Game
{
    public class GameInstance<T> : IGameInstance<T> where T: IGameSpecificPlayerData
    {
        public SessionPlayers<T> Players { get; } = new();
    }
}
