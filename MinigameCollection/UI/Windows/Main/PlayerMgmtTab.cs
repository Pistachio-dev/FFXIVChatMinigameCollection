using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using ECommons;
using System.Numerics;

namespace MinigameCollection.UI.Windows.Main
{
    internal class PlayerMgmtTab : PluginWindowBase
    {
        private readonly GameHost host;
        private readonly PlayerManager playerMng;

        public PlayerMgmtTab(GameHost host, PlayerManager playerMng, ILogService logService, string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) 
            : base(logService, name, flags, forceMainWindow)
        {
            this.host = host;
            this.playerMng = playerMng;
        }

        protected override void SafeDraw()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 6, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableSetupColumn("Bet", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Funds", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Cards", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableHeadersRow();

                var playerCounter = 0;
                foreach (var player in host.Players.Players)
                {
                    ImGui.TableNextRow();
                    var color = GetRowColor(playerCounter);
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, color);

                    ImGui.TableNextColumn();
                    ImGui.BeginGroup();
                    var playerName = player.FullName;
                    ImGui.TextUnformatted(playerName);
                    ImGui.EndGroup();
                }

                ImGui.EndTable();
            }

            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Plus, "Add targeted player"))
            {
                playerMng.TryAddTargetedPlayer();
            }

            DrawTooltip("Add the player you're currently targeting to the game.");
        }

        private uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }
    }
}
