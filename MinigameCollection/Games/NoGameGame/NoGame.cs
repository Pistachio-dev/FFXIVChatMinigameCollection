using Dalamud.Bindings.ImGui;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.NoGameGame
{
    internal class NoGame : Game
    {
        public static GameId Id => new GameId("No Game");
        private PlayerSet players;

        public override void DrawUI()
        {
            ImGui.TextUnformatted("No game selected");
            foreach (var player in players.Players)
            {
                ImGui.TextUnformatted(player.FullName);
            }
        }

        public override void Initialize(PlayerSet players)
        {
            this.players = players;
        }

        public override void Update()
        {
            if (players == null) return;
        }
    }
}
