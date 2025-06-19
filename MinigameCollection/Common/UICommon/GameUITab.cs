using ImGuiNET;
using MinigameCollection.Common.GameActionsCommon;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.UICommon
{
    public abstract class GameUITab
    {
        protected readonly Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1);
        private readonly GameBoardBase gameBoard;
        private readonly GameActionsBase gameActions;

        public GameUITab(GameBoardBase gameBoard, GameActionsBase gameActions)
        {
            this.gameBoard = gameBoard;
            this.gameActions = gameActions;
        }

        public virtual void Draw()
        {
            DrawDefault();
        }

        protected void DrawDefault()
        {
            ImGui.TextUnformatted("No game selected");
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##GamePlayerTable", 2, flags))
            {
                ImGui.TableSetupColumn("Player name", ImGuiTableColumnFlags.WidthStretch, 0.7f);
                ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.3f);
            }
            ImGui.TableHeadersRow();
            var playerIndex = 0;
            foreach (var player in gameBoard.Players)
            {
                ImGui.TableNextRow();
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(defaultColor));

                ImGui.TableNextColumn();
                ImGui.TextUnformatted(player.FullName);

                ImGui.TableNextColumn();
                ImGui.TextUnformatted("Placeholder");
            }

            ImGui.EndTable();
        }
    }
}
