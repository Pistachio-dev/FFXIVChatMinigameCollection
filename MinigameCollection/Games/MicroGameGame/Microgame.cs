using Dalamud.Bindings.ImGui;
using MinigameCollection;
using MinigameCollection.Games;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.MicroGameGame
{
    public class Microgame : Game
    {
        public static GameId Id => new GameId("Microgame");

        private PlayerSet players;       

        public override void DrawUI()
        {
            ImGui.TextUnformatted("This is the microgame");
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
