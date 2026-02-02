using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using Dalamud.Plugin.Services;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using ECommons;
using MinigameCollection.Bank;
using MinigameCollection.Trader;
using Model.Base;
using System.Numerics;

namespace MinigameCollection.UI.Windows.Main.PlayerManagement
{
    internal partial class PlayerMgmtTab : PluginWindowBase
    {
        private readonly GameHost host;
        private readonly PlayerManager playerMgmt;
        private readonly BankActions bankMgmt;
        private readonly ColorPalette palette;
        private readonly TradingManager tradingManager;

        public PlayerMgmtTab(GameHost host, PlayerManager playerMng, BankActions bankMgmt, ILogService logService, string name,
            ColorPalette palette, TradingManager tradingManager,
            ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false) 
            : base(logService, name, flags, forceMainWindow)
        {
            this.host = host;
            this.playerMgmt = playerMng;
            this.bankMgmt = bankMgmt;
            this.palette = palette;
            this.tradingManager = tradingManager;
        }


        protected override void SafeDraw()
        {
            const ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders;
            if (ImGui.BeginTable("##PlayerTable", 5, flags))
            {
                ImGui.TableSetupColumn("Player", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableSetupColumn("AFK", ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableSetupColumn("Gil in a game", ImGuiTableColumnFlags.WidthStretch, 0.2f);
                ImGui.TableSetupColumn("Stored gil", ImGuiTableColumnFlags.WidthStretch, 0.3f);
                ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthStretch, 0.8f);
                ImGui.TableHeadersRow();

                var playerCounter = 0;
                foreach (var player in host.Players.Players)
                {
                    ImGui.TableNextRow();
                    var color = GetRowColor(playerCounter);
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, color);

                    // Player name
                    ImGui.TableNextColumn();
                    ImGui.BeginGroup();
                    var playerName = player.FullName;
                    ImGui.TextUnformatted(playerName);
                    ImGui.EndGroup();

                    // AFK
                    ImGui.TableNextColumn();
                    ImGui.TextColored(player.Afk ? palette.MidRed : palette.MidGreen, player.Afk ? "AFK" : "No");

                    // Guil in game
                    ImGui.TableNextColumn();
                    ImGui.TextColored(player.Bank.InUse >= 0 ? palette.White : palette.MidRed, player.Bank.InUse.Formatted());

                    // Gil stored
                    ImGui.TableNextColumn();
                    ImGui.TextColored(player.Bank.Stored >= 0 ? palette.White : palette.MidRed, player.Bank.Stored.Formatted());

                    // Actions
                    ImGui.TableNextColumn();
                    DrawPlayerActionButtons(player);

                    playerCounter++;
                }

                ImGui.EndTable();
            }

            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Plus, "Add targeted player"))
            {
                playerMgmt.TryAddTargetedPlayer();
            }
            ImGui.SameLine();
            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.DollarSign, "Manage gil")) ImGui.OpenPopup($"Funds");

            DrawTooltip("Add the player you're currently targeting to the game.");
            DrawFundsModal();

        }

        private uint GetRowColor(int row)
        {
            return ImGui.GetColorU32(new Vector4(0.3f, 0.3f, 0.3f, row % 2 != 0 ? 0.65f : 0.45f));
        }

        private void DrawPlayerActionButtons(MGPlayer player)
        {
            Vector2 buttonSize = new Vector2(18, 18);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(1, 0));
            ImGui.PushID($"AFKButton#{player.FullName}");
            if (ImGui.Button("", buttonSize))
            {
                playerMgmt.TogglePlayerAFK(playerMgmt.GetPlayer(player.FullName));
            }
            DrawTooltip("Toggle AFK status. AFK players keep their funds, but don't play.");

            ImGui.PushID($"SmallWakeUpButton#{player.FullName}");
            ImGui.SameLine();
            if (ImGui.Button($"!", buttonSize))
            {
                playerMgmt.ChatSoundWakeUp(player);
            }
            DrawTooltip("Play wake up sound");
            ImGui.SameLine();
            if (ImGuiComponents.IconButton("##buyin{player.FullName}", Dalamud.Interface.FontAwesomeIcon.ArrowDown)) {
                tradingManager.StartBuyIn(player);
            }
            DrawTooltip("Buy in (take gil from player and into their bank");
            ImGui.SameLine();
            if (ImGuiComponents.IconButton($"##cashout{player.FullName}", Dalamud.Interface.FontAwesomeIcon.ArrowUp))
            {
                tradingManager.StartCashOut(player);
            }
            DrawTooltip("Cash out (take gil from player bank and to the player)");
            ImGui.SameLine();
            if (ImGuiComponents.IconButton($"secure##{player.FullName}", Dalamud.Interface.FontAwesomeIcon.SackDollar))
            {
                bankMgmt.StoreAll(player);
            }
            DrawTooltip("Move any gil in a game into the bank");
            ImGui.PopStyleVar();
            ImGui.PopID();
        }
    }
}
