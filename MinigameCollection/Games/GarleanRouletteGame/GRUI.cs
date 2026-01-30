using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Games.MicroGameGame;
using MinigameCollection.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Dalamud.Interface.Utility.Raii.ImRaii;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRUI
    {
        private readonly GameHost host;
        private readonly ColorPalette palette;
        private readonly GRGameState gameState;
        private readonly GRActions grActions;

        public GRUI(GameHost host, ColorPalette palette, GRGameState gameState, GRActions grActions)
        {
            this.host = host;
            this.palette = palette;
            this.gameState = gameState;
            this.grActions = grActions;
        }

        public void DrawUI()
        {
            DrawPlayerTable();
            DrawButtons();
            if (gameState.Stage == GRStage.Shooting)
            {
                DrawPlayerOrder();
                DrawChambersLoaded();
                DrawCurrentPlayer();
            }
            
        }

        private void DrawPlayerOrder()
        {
            ImGui.TextUnformatted($"Order: {host.Players.GetNonAfkPlayers().Select(p => p.FullName.GetFirstName()).ToList().GetWordsSeparatedByArrows()}");
        }

        private void DrawPlayerTable()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 2, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableHeadersRow();

                var playerCounter = 0;
                foreach (var player in host.Players.Players)
                {
                    ImGui.TableNextRow();
                    var color = palette.GetRowColor(playerCounter);
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, color);

                    ImGui.TableNextColumn();
                    ImGui.BeginGroup();
                    var playerName = player.FullName.GetNameOnly();
                    ImGui.TextUnformatted(playerName);
                    ImGui.EndGroup();

                    // Alive?
                    ImGui.TableNextColumn();
                    if (player.GetData().Alive) { ImGui.TextColored(palette.LightGreen, "OK"); }
                    else { ImGui.TextColored(palette.LightRed, "KO"); }                                                
                }

                ImGui.EndTable();
            }
        }

        private void DrawButtons()
        {
            if (gameState.Stage == GRStage.Shooting)
            {
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Gun, "Take next shot manually"))
                {
                    grActions.RollInsteadOfPlayer();
                }
            }

            if (gameState.Stage == GRStage.NotStarted)
            {
                if (ImGui.Button(" Roll for player order"))
                {
                    grActions.StartOrderRound();
                }
            }
            if (gameState.Stage == GRStage.Winner)
            {
                ImGui.TextColored(palette.LightGreen, $"{gameState.GetSurvivor().FullName} wins!");
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Repeat, "Go again"))
                {
                    grActions.StartOrderRound();
                }
            }
        }

        private void DrawChambersLoaded()
        {
            if (gameState.ChambersLoaded.Any()) {
                ImGui.TextUnformatted($"Chambers loaded: {gameState.ChambersLoaded.OrderBy(c => c).Humanize()}");
            }
            
        }

        private void DrawCurrentPlayer()
        {
            ImGui.TextUnformatted($"Up next: {(gameState.CurrentPlayer?.FullName ?? "Nobody")}");            
        }
    }
}
