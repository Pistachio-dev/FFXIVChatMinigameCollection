using CommonServices.PlayerManagement.Interface;
using Dalamud.Bindings.ImGui;
using System;

namespace MinigameCollection.Windows.Main
{
    public partial class MainWindow
    {
        // Handle the common handling of players
        private void DrawPlayerManagementTab(ISessionPlayerManager mgr)
        {
            ImGui.TextUnformatted("No game selected");
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##GamePlayerTable", 3, flags))
            {
                ImGui.TableSetupColumn("Player name", ImGuiTableColumnFlags.WidthStretch, 0.7f);
                ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.3f);

                ImGui.TableHeadersRow();
                var playerIndex = 0;
                foreach (var player in mgr.GetPlayersPlaying())
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, ImGui.GetColorU32(defaultColor));

                    // Player Name
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.FullName);

                    // Actions
                    ImGui.TableNextColumn();
                    if (ImGui.Button($"##{playerIndex}"))
                    {
                        RunAfterDraw(() => mgr.RemovePlayer(player.FullName));
                    }

                    playerIndex++;
                }
            }
            ImGui.EndTable();

            if (ImGui.Button("Add target player"))
            {
                mgr.AddTargetPlayer();
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
