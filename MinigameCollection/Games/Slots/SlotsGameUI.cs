using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using MinigameCollection.Games.GarleanRouletteGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Slots
{
    public class SlotsGameUI
    {
        private readonly SlotsGameActions actions;
        private readonly SlotsGameState state;

        public SlotsGameUI(SlotsGameActions actions, SlotsGameState state)
        {
            this.actions = actions;
            this.state = state;
        }
        public void DrawUI()
        {
            ImGui.TextUnformatted("Players type \"bet <amount> to roll the slots. For instance: \"bet 32k\" or \"bet 1m\"");
            switch (state.Stage)
            {
                case SlotsGameStage.Idle:
                    ImGui.TextUnformatted("Status: Waiting");
                    break;
                case SlotsGameStage.Rolling:
                    ImGui.TextUnformatted("Status: Spinning");
                    break;
                case SlotsGameStage.ShowingResult:
                    ImGui.TextUnformatted("Status: Showing results");
                    break;                    
            }

            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Info, "Print payouts table"))
            {
                actions.PrintPayoutTable();
            }

            ImGui.BeginDisabled(!ImGui.GetIO().KeyShift);
            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Trash, "Reset"))
            {
                actions.Reset();
            }
            DrawTooltip("Shift+Click to reset the game.");
            ImGui.EndDisabled();
        }

        protected void DrawTooltip(string text)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(text);
            }
        }
    }
}
