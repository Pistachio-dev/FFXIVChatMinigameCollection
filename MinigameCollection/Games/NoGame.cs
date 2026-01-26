using Dalamud.Bindings.ImGui;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    internal class NoGame : IGame
    {
        public GameId Id => new GameId("No game");
        private PlayerSet players;

        public void DrawUI()
        {
            ImGui.TextUnformatted("No game selected");
            foreach (var player in players.Players)
            {
                ImGui.TextUnformatted(player.Fullname);
            }
        }

        public void Initialize(PlayerSet players)
        {
            this.players = players;
        }

        public void Update()
        {
            if (players == null) return;
        }
    }
}
