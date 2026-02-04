using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Extensions;
using Model.Base;
using System;
using System.Numerics;

namespace MinigameCollection.Games.MicroGameGame
{
    public class Microgame : Game
    {
        public static string Description { get; } = "Very simple game where the button rolls a random point for a random player until someone reaches a limit.\nMade for testing.";
        public Microgame()
        {
        }

        public static GameId Id => new GameId("Microgame");

        private PlayerSet playerSet;
        private GameHost host;
        private MicroGameState state = MicroGameState.NotStarted;
        private MGPlayer? winner = null;

        public override void DrawUI()
        {
            ImGui.TextUnformatted("This is the microgame");
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 6, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("Score", ImGuiTableColumnFlags.WidthStretch, 0.3f);
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
                    var playerName = player.FullName.GetNameOnly();
                    ImGui.TextUnformatted(playerName);
                    ImGui.EndGroup();

                    // Score
                    ImGui.TableNextColumn();
                    ImGui.TextUnformatted(player.GetData<MicroGamePlayerData>(Id).Score.ToString());
                }

                ImGui.EndTable();
            }

            switch (state)
            {
                case MicroGameState.Playing:
                    if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Dice, "Roll random point"))
                    {
                        var player = this.playerSet.Players[new Random().Next(0, this.playerSet.Players.Count)];
                        var data = player.GetData<MicroGamePlayerData>(Id);
                        data.Score += 1;
                        player.SetData(Id, data);

                        if (data.Score >= 5)
                        {
                            winner = player;
                            state = MicroGameState.WinnerFound;
                        }
                    }
                    break;

                case MicroGameState.WinnerFound:
                    {
                        ImGui.TextUnformatted($"{winner?.FullName.GetFirstName() ?? "null?"} wins");
                        break;
                    }
                case MicroGameState.NotStarted:
                    if (playerSet.Players.Count > 1)
                    {
                        if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.ArrowRight, "Start this random microgame"))
                        {
                            state = MicroGameState.Playing;
                        }
                        break;
                    }
                    ImGui.TextUnformatted("Waiting for players");
                    break;
            }
        }

        public override void Initialize(GameHost host)
        {
            this.host = host;
            this.playerSet = host.Players;
            foreach (var player in this.playerSet.Players)
            {
                player.SetData(Id, new MicroGamePlayerData() { Score = 0 });
            }
        }

        public override void Update()
        {
        }

        private uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }
    }
}
