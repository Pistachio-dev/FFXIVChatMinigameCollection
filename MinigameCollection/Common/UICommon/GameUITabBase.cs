using Dalamud.Bindings.ImGui;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MinigameCollection.Common.UICommon
{
    public class GameUITabBase : PluginWindowBase
    {
        protected readonly Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        private readonly SessionPlayerManager players;
        private readonly GameBase gameBoard;
        private List<Action> delayedActions = new(); // For actions that can't be done while iterating, like removing a player

        public GameUITabBase(ILogService logService, SessionPlayerManager players, GameBase gameBase) : base(logService, "##DefaultGameUITab")
        {
            this.players = players;
            this.gameBoard = gameBase;
        }

        public virtual void Draw()
        {
            DrawDefault();
        }

        protected void DrawDefault()
        {
            ImGui.TextUnformatted("No game selected");
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##GamePlayerTable", 3, flags))
            {
                ImGui.TableSetupColumn("Player name", ImGuiTableColumnFlags.WidthStretch, 0.7f);
                ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.3f);

                ImGui.TableHeadersRow();
                var playerIndex = 0;
                foreach (var player in gameBoard.playerManager.GetPlayersPlaying())
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(defaultColor));

                    // Player Name
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.FullName);

                    // Status
                    ImGui.TableNextColumn();
                    (var text, var color) = GetStatusAndColor(player);
                    ImGui.TextColored(color, text);

                    // Actions
                    ImGui.TableNextColumn();
                    if (ImGui.Button($"##{playerIndex}"))
                    {
                        RunAfterDraw(() => players.RemovePlayer(player.FullName));
                    }

                    playerIndex++;
                }
            }
            ImGui.EndTable();

            if (ImGui.Button("Add target player"))
            {
                players.AddTargetPlayer();
            }

            RunDelayedActions();
        }

        protected void RunAfterDraw(Action action)
        {
            delayedActions.Add(action);
        }

        protected void RunDelayedActions()
        {
            foreach (var action in delayedActions)
            {
                action();
            }

            delayedActions.Clear();
        }

        protected virtual (string, Vector4) GetStatusAndColor(PlayerInSession player)
        {
            if (player.IsAFK)
            {
                return ("AFK", Colors.GreyHalf);
            }

            return ("Ready", Colors.GreenHalf);
        }
    }
}
