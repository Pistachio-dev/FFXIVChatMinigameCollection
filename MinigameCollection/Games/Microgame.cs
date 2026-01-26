using Dalamud.Bindings.ImGui;
using MinigameCollection;
using MinigameCollection.Games;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Microgame
{
    public class Microgame : IGame
    {
        public GameId Id => new GameId("Microgame");

        private PlayerSet players;

        public void DrawUI()
        {
            ImGui.TextUnformatted("This is the microgame");
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
