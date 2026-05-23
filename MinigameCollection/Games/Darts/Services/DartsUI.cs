using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using MinigameCollection.Games.GarleanRouletteGame;
using MinigameCollection.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsUI
    {
        private readonly GameHost host;
        private readonly ColorPalette palette;
        private readonly DartsGameState gameState;
        private readonly DartsActions actions;

        public DartsUI(GameHost host, ColorPalette palette, DartsGameState gameState, DartsActions actions)
        {
            this.host = host;
            this.palette = palette;
            this.gameState = gameState;
            this.actions = actions;
        }

        public void DrawUI()
        {
            ImGui.Text("Not implemented yet");
            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Play, "Start Game"))
            {
                actions.StartOrderRound();
            }
        }
    }
}
