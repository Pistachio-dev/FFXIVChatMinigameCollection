using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using System.Numerics;

namespace MinigameCollection.UI.Windows.Main.PlayerManagement
{
    internal partial class PlayerMgmtTab
    {
        private bool isFundsDialogueOpen = false;
        private int selectedPlayerNameIndex = -1;
        private int addOrRemove = 0; // 0 = add, 1 = remove.
        private int amountToAddOrRemove = 0;
        private int amountToSetAllPlayersTo = 0;

        private void DrawFundsModal()
        {
            //var center = ImGui.GetMainViewport().GetCenter();
            //ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));

            isFundsDialogueOpen = true;
            if (ImGui.BeginPopupModal("Funds", ref isFundsDialogueOpen, ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.TextColored(new Vector4(1, 0, 0, 1), "This dialog is meant to be used to play without actual gil or troubleshoot trade errors. For gil, please shift click the player funds to" +
                    " ask for gil, shift righ-click to cash out.");
                ImGui.TextUnformatted("Select a player");
                var playerNames = host.Players.Players.Select(p => p.FullName).ToArray();
                ImGui.Combo("Player", ref selectedPlayerNameIndex, playerNames, playerNames.Length);

                if (selectedPlayerNameIndex != -1)
                {
                    ImGui.RadioButton("Add funds", ref addOrRemove, 0); ImGui.SameLine();
                    ImGui.RadioButton("Cash out", ref addOrRemove, 1);
                    ImGui.InputInt(addOrRemove == 0 ? "Amount to add" : "Amount to cash out", ref amountToAddOrRemove);

                    var selectedPlayerName = playerNames[selectedPlayerNameIndex];
                    var player = playerMgmt.GetPlayer(selectedPlayerName, true);
                    var buttonText = addOrRemove == 0 ? "Add gil" : "Remove gil";
                    DrawActionButton(() => bankMgmt.AddStored(playerMgmt.GetPlayer(selectedPlayerName), amountToAddOrRemove * (addOrRemove == 0 ? 1 : -1)), buttonText);
                    ImGui.SameLine();
                    DrawActionButton(() => bankMgmt.SetStored(player, 0), "Remove all");
                    ImGui.SameLine();
                    if (ImGui.Button("Close##1"))
                    {
                        ImGui.CloseCurrentPopup();
                    }
                }
                ImGui.Separator();
                ImGui.TextUnformatted("Or, for a quick game");
                ImGui.InputInt("Set all players' funds to this amount", ref amountToSetAllPlayersTo);
                if (ImGui.Button("Set all funds"))
                {
                    bankMgmt.SetAllStored(host.Players, amountToSetAllPlayersTo);
                }
                ImGui.SameLine();
                if (ImGui.Button("Close##2"))
                {
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }
        }        
    }
}
