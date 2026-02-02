using Dalamud.Bindings.ImGui;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using ECommons.Configuration;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Bank;
using MinigameCollection.Games.NoGameGame;
using MinigameCollection.Trader;
using MinigameCollection.UI;
using MinigameCollection.UI.Windows.Main.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace MinigameCollection.Windows.Main;

public partial class MainWindow : PluginWindowBase, IDisposable
{
    protected readonly Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1);


    private IDataManager dataManager;
    private IChatOutput chatOutput;
    private IObjectTable objectTable;
    private IConfigurationService<Configuration> configurationSvc;
    private INotificationManager notificationManager;
    private List<System.Action> delayedActions = new(); // For actions that can't be done while iterating, like removing a player
    private GameHost gameHost;
    private BankActions bankMng;
    private PlayerManager playerManager;
    Configuration configuration;
    TradingManager tradingMgr;
    PlayerMgmtTab playerMgmtTab;
    TradingManager tradingManager;
    
    ColorPalette palette;


    public MainWindow(ILogService logService, IServiceProvider serviceProvider)
        : base(logService, "MinigameCollection")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(700, 360),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        dataManager = serviceProvider.GetRequiredService<IDataManager>();
        chatOutput = serviceProvider.GetRequiredService<IChatOutput>();
        objectTable = serviceProvider.GetRequiredService<IObjectTable>();
        notificationManager = serviceProvider.GetRequiredService<INotificationManager>();
        configurationSvc = serviceProvider.GetRequiredService<IConfigurationService<Configuration>>();
        gameHost = serviceProvider.GetRequiredService<GameHost>();
        configuration = configurationSvc.GetConfiguration();
        playerManager = serviceProvider.GetRequiredService<PlayerManager>();
        palette = serviceProvider.GetRequiredService<ColorPalette>();
        bankMng = serviceProvider.GetRequiredService<BankActions>();
        tradingManager = serviceProvider.GetRequiredService<TradingManager>();
        playerMgmtTab = new PlayerMgmtTab(gameHost, playerManager, bankMng, logService, "Player Management", palette, tradingManager);
        tradingMgr = serviceProvider.GetRequiredService<TradingManager>();
        
    }

    public void Dispose() { }

    private string queryedSheetName = "";

    protected override void SafeDraw()
    {
        if (!gameHost.HasGame())
        {
            gameHost.StartGame(gameHost.AvailableGames()[configuration.SelectedGame].id);
        }

        if (ImGui.Button("Trade info"))
        {
            tradingMgr.PrintInfo();
        }
        if (ImGui.Button("Press Yes"))
        {
            tradingMgr.SelectYesBruteForce();
        }


        ImGuiTabBarFlags tabBarFlags = ImGuiTabBarFlags.None;
        if (ImGui.BeginTabBar("##Main container"))
        {
            if (ImGui.BeginTabItem("Game"))
            {
                gameHost.DrawUI();

                ImGui.EndTabItem();
                
            }
            if (ImGui.BeginTabItem("Players"))
            {
                playerMgmtTab.Draw();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Gil & Bank"))
            {
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Game select"))
            {
                int selected = configurationSvc.GetConfiguration().SelectedGame;
                var availableGames = gameHost.AvailableGames();
                if (ImGui.Combo("Game mode", ref selected, availableGames.Select(data => data.id.Value).ToArray(), availableGames.Length))
                {
                    if (gameHost.HasGame())
                    {

                    }
                    configuration.SelectedGame = selected;
                    configurationSvc.SaveConfiguration();
                    gameHost.StartGame(availableGames[selected].id);
                }

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
