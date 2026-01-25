using CommonServices.PlayerManagement.Interface;
using MinigameCollection.Common.GameBoardCommon;
using Model.PlayerManagement;

namespace MinigameCollection.Games.NoGame
{
    internal class NoGamePlayerInSession : PlayerInSession<IGameSpecificPlayerData>
    {
        public NoGamePlayerInSession(PlayerOOGData player) : base(player)
        {
        }

        public string AdditionalProperty1 { get; set; } = "property from subclass";
    }
}
