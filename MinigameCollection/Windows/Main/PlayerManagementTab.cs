using System;

namespace MinigameCollection.Windows.Main
{
    public partial class MainWindow
    {
        // Handle the common handling of players
        private void DrawPlayerManagementTab()
        {
            //ImGui.TextUnformatted("No game selected");
            //const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            //if (ImGui.BeginTable("##GamePlayerTable", 3, flags))
            //{
            //    ImGui.TableSetupColumn("Player name", ImGuiTableColumnFlags.WidthStretch, 0.7f);
            //    ImGui.TableSetupColumn("Status", ImGuiTableColumnFlags.WidthStretch, 0.3f);
            //    ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.3f);

            //    ImGui.TableHeadersRow();
            //    var playerIndex = 0;
            //    foreach (var player in playersInSessionManager.InGame)
            //    {
            //        ImGui.TableNextRow();
            //        ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(defaultColor));

            //        // Player Name
            //        ImGui.TableNextColumn();
            //        ImGui.TextUnformatted(player.FullName);

            //        // Status
            //        ImGui.TableNextColumn();
            //        (var text, var color) = GetStatusAndColor(player);
            //        ImGui.TextColored(color, text);

            //        // Actions
            //        ImGui.TableNextColumn();
            //        if (ImGui.Button($"##{playerIndex}"))
            //        {
            //            RunAfterDraw(() => playersInSessionManager.RemovePlayer(player.FullName));
            //        }

            //        playerIndex++;
            //    }
            //}
            //ImGui.EndTable();

            //if (ImGui.Button("Add target player"))
            //{
            //    playersInSessionManager.AddTargetPlayer();
            //}

            //RunDelayedActions();
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

        //protected virtual (string, Vector4) GetStatusAndColor(PlayerInSession player)
        //{
        //    if (player.IsAFK)
        //    {
        //        return ("AFK", Colors.GreyHalf);
        //    }

        //    return ("Ready", Colors.GreenHalf);
        //}    
    }
}
