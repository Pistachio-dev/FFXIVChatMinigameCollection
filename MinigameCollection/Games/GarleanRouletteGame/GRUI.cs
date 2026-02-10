using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.UI;
using System.Linq;

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
            ImGui.TextUnformatted("Garlean roulette");
            DrawPlayerTable();
            if (gameState.Stage == GRStage.NotStarted)
            {
                DrawBetSetting();
            }
            DrawButtons();
            if (gameState.Stage == GRStage.Shooting)
            {
                DrawPlayerOrder();
                DrawChambersLoaded();
                DrawCurrentPlayer();
            }           
        }

        private void DrawBetSetting()
        {
            var previous = gameState.Bet;
            if (ImGui.InputLong("Bet", ref previous))
            {
                gameState.Bet = previous;
            }
            
            ImGui.SameLine();
            if (ImGui.Button("Announce bet"))
            {
                host.ChatOutput.WriteChat($"Bet set to {gameState.Bet.Formatted()}");
            }
        }

        private void DrawPlayerOrder()
        {
            ImGui.TextUnformatted($"Order: {host.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).ToList().GetWordsSeparatedByArrows()}");
        }

        private void DrawPlayerTable()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 4, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Bet", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Bank", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableHeadersRow();

                var playerCounter = 0;
                foreach (var player in host.Players.ActivePlayers)
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

                    // Bet
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.Bank.InUse.Formatted());

                    // Bank
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.Bank.Stored.Formatted());
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
                DrawTooltip("Roll /dice 100 for each player. Players will shoot from lower to higher roll");

            }
            if (gameState.Stage == GRStage.Winner)
            {
                var survivor = gameState.GetSurvivor();
                if (survivor == null)
                {
                    ImGui.TextUnformatted("No survivors. This is not supposed to happen");
                }
                else
                {                
                    ImGui.TextColored(palette.LightGreen, $"{survivor.FullName} wins! <se.15>");
                }
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Repeat, "Go again"))
                {
                    grActions.GoBackToBetting();
                }
            }

            ImGui.SameLine();
            ImGui.BeginDisabled(!ImGui.GetIO().KeyShift);
            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Trash, "Reset"))
            {
                grActions.ResetGame(host);
            }
            DrawTooltip("Shift+Click to reset the game.");
            ImGui.EndDisabled();
            
        }

        private void DrawChambersLoaded()
        {
            if (gameState.ChambersLoaded.Any())
            {
                ImGui.TextUnformatted($"Chambers loaded: {gameState.ChambersLoaded.OrderBy(c => c).Humanize()}");
            }
        }

        private void DrawCurrentPlayer()
        {
            ImGui.TextUnformatted($"Up next: {(gameState.CurrentPlayer?.FullName ?? "Nobody")}");
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
