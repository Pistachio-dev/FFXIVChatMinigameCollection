using Dalamud.Bindings.ImGui;
using Dalamud.Interface.ImGuiNotification;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Lumina.Excel.Sheets;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Common;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MinigameCollection.Windows.Main;

public partial class MainWindow : PluginWindowBase, IDisposable
{
    protected readonly Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1);


    private IDataManager dataManager;
    private IChatOutput chatOutput;
    private IObjectTable objectTable;
    private INotificationManager notificationManager;
    private GameModeManager gameModeManager;
    private PlayersInSessionManager playersInSessionManager;
    private List<System.Action> delayedActions = new(); // For actions that can't be done while iterating, like removing a player


    public MainWindow(ILogService logService, IServiceProvider serviceProvider)
        : base(logService, "MinigameCollection", ImGuiWindowFlags.AlwaysAutoResize)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(600, 360),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        dataManager = serviceProvider.GetRequiredService<IDataManager>();
        chatOutput = serviceProvider.GetRequiredService<IChatOutput>();
        objectTable = serviceProvider.GetRequiredService<IObjectTable>();
        notificationManager = serviceProvider.GetRequiredService<INotificationManager>();
        gameModeManager = serviceProvider.GetRequiredService<GameModeManager>();
        playersInSessionManager = serviceProvider.GetRequiredService<PlayersInSessionManager>();
    }

    public void Dispose() { }

    private string queryedSheetName = "";

    protected override void SafeDraw()
    {
        ImGuiTabBarFlags tabBarFlags = ImGuiTabBarFlags.None;
        if (ImGui.BeginTabBar("##Main container"))
        {
            if (ImGui.BeginTabItem("Game"))
            {
                gameModeManager.GetGame(GameSelected.None).Draw();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Players"))
            {
                DrawPlayerManagementTab();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Gil & Bank"))
            {
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Game select"))
            {
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Experimental"))
            {
                DrawExperimentalButtons();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }
    }

    private unsafe void ListFates()
    {
        var fm = *FateManager.Instance();
        var fatePointers = fm.Fates;
        foreach (var pointer in fatePointers)
        {
            var fc = *pointer.Value;
            logService.Warning($"{fc.Name}");
        }
    }

    private unsafe void ListMarkers()
    {
        var map = *FFXIVClientStructs.FFXIV.Client.Game.UI.Map.Instance();
        foreach (var markerInfo in map.QuestMarkers)
        {
            foreach (var data in markerInfo.MarkerData)
            {

            }
        }
    }
}
