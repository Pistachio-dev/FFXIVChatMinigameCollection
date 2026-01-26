using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using ECommons;
using System;
using System.Linq;
using System.Numerics;
using System.Xml;

namespace MinigameCollection.UI.Windows.Main
{
    internal class PlayerMgmtTab : PluginWindowBase
    {
        private readonly GameHost host;

        public PlayerMgmtTab(GameHost host, ILogService logService, string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) 
            : base(logService, name, flags, forceMainWindow)
        {
            this.host = host;
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

                }

                ImGui.EndTable();
            }
        }

        private uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }
    }
}
