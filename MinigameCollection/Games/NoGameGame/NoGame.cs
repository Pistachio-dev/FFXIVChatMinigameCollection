using Dalamud.Bindings.ImGui;
using Model.Base;

namespace MinigameCollection.Games.NoGameGame
{
    internal class NoGame : Game
    {
        public static GameId Id => new GameId("No Game");
        public static string Description { get; } = "No game is selected.";
        private PlayerSet players => gameHost.Players;
        private GameHost gameHost;

        public NoGame()
        {
        }

        public override void DrawUI()
        {
            ImGui.TextUnformatted("No game selected. Select on on the \"Game select\" tab");
            foreach (var player in players.AllPlayers)
            {
                ImGui.TextUnformatted(player.FullName);
            }
        }

        public override void Initialize(GameHost host)
        {
            this.gameHost = host;
        }

        public override void Update()
        {
            if (players == null) return;
        }
    }
}
