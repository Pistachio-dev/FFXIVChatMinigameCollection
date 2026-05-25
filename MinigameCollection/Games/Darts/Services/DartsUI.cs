using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Extensions;
using MinigameCollection.Bank;
using MinigameCollection.UI;
using System.Linq;

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
            DrawPlayerTable();
            if (gameState.Stage != DartsStage.BeforeGame && host.Players.ActivePlayers.FirstOrDefault()?.GetData().OrderRolled != -1)
            {
                DrawPlayerOrder();
            }
            
            DrawCurrentPlayer();
            if (gameState.Stage == DartsStage.BeforeGame || gameState.Stage == DartsStage.ShowingWinners)
            {
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Play, "Start Game"))
                {
                    actions.StartOrderRound();
                }
                DrawBetSetting();
            }
        }

        private void DrawCurrentPlayer()
        {
            ImGui.TextUnformatted($"Now throwing: {(gameState.CurrentPlayer?.FullName ?? "Nobody")}");
        }

        private void DrawPlayerTable()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 5, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Position", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Score", ImGuiTableColumnFlags.WidthStretch, 0.3f);
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

                    // Position
                    ImGui.TableNextColumn();
                    string place = player.GetData().Place switch
                    {
                        -1 => string.Empty,
                        0 => string.Empty,
                        1 => "1st",
                        2 => "2nd",
                        3 => "3rd",
                        _ => player.GetData().Place + "th"
                    };
                    ImGui.Text(place);

                    // Score
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.GetData().Score.ToString());

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

        protected void DrawTooltip(string text)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(text);
            }
        }
    }
}
