using Dalamud.Bindings.ImGui;
using DalamudBasics.Extensions;
using MinigameCollection;
using MinigameCollection.Games;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MinigameCollection.Games.MicroGameGame
{
    public class Microgame : Game
    {
        public Microgame()
        {

        }
        public static GameId Id => new GameId("Microgame");

        private PlayerSet playerSet;
        private GameHost host;

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
            if (playerSet == null) return;
        }
        private uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }

    }
}
