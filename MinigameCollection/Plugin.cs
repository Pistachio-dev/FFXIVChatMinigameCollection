using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Listener;
using DalamudBasics.Chat.Output;
using DalamudBasics.Debugging;
using DalamudBasics.DependencyInjection;
using DalamudBasics.Interop;
using DalamudBasics.Logging;
using ECommons;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Dice;
using MinigameCollection.Games.GarleanRouletteGame;
using MinigameCollection.Games.MicroGameGame;
using MinigameCollection.Games.NoGameGame;
using MinigameCollection.UI;
using MinigameCollection.UI.Windows;
using MinigameCollection.Windows.Main;
using System;

namespace MinigameCollection;

public sealed class Plugin : IDalamudPlugin
{
    private const string CommandName = "/minig";
    private const string WaterMark = "[MG]";

    [PluginService]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    internal static IPluginLog Log { get; private set; }
    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("MinigameCollection");
    private ConfigWindow ConfigWindow { get; init; }
    private MainWindow MainWindow { get; init; }
    private IServiceProvider serviceProvider { get; init; }
    private ILogService logService { get; set; }

    private PlayerSet players { get; set; }

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this);

        players = new PlayerSet();
        serviceProvider = BuildServiceProvider(pluginInterface);
        
        logService = serviceProvider.GetRequiredService<ILogService>();        

        InitializeServices(serviceProvider);

        ConfigWindow = new ConfigWindow(logService, serviceProvider);
        MainWindow = new MainWindow(logService, serviceProvider);

        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(MainWindow);

        serviceProvider.GetRequiredService<ICommandManager>().AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Type /minig to start"
        });

        pluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        pluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        pluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
    }

    public void Dispose()
    {
        serviceProvider.GetRequiredService<HookManager>().Dispose();
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        MainWindow.Dispose();

        serviceProvider.GetRequiredService<ICommandManager>().RemoveHandler(CommandName);        
    }

    private IServiceProvider BuildServiceProvider(IDalamudPluginInterface pluginInterface)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddAllDalamudBasicsServices<Configuration>(pluginInterface);
        serviceCollection.AddSingleton<StringDebugUtils>();
        serviceCollection.AddSingleton<PlayerManager>();
        serviceCollection.AddSingleton<PlayerSet>((sp) => this.players);
        serviceCollection.AddSingleton<GameHost>();
        serviceCollection.AddSingleton<RollTracker>();
        serviceCollection.AddSingleton<IServiceProvider>((sp) => serviceProvider);
        serviceCollection.AddSingleton<GarleanRoulette>();
        serviceCollection.AddSingleton<GRActions>();
        serviceCollection.AddSingleton<GRGameState>();
        serviceCollection.AddSingleton<GRChatOutput>();
        serviceCollection.AddSingleton<GRUI>();
        serviceCollection.AddSingleton<NoGame>();
        serviceCollection.AddSingleton<Microgame>();
        serviceCollection.AddSingleton<MicroGamePlayerData>();
        serviceCollection.AddSingleton<ColorPalette>();
        //serviceCollection.AddGamesBase();        
        //serviceCollection.AddNoGame();

        return serviceCollection.BuildServiceProvider();
    }

    private void InitializeServices(IServiceProvider serviceProvider)
    {
        IFramework framework = serviceProvider.GetRequiredService<IFramework>();
        serviceProvider.GetRequiredService<ILogService>().AttachToGameLogicLoop();
        serviceProvider.GetRequiredService<IChatListener>().InitializeAndRun(WaterMark, true);
        serviceProvider.GetRequiredService<IChatOutput>().InitializeAndAttachToGameLogicLoop(framework, WaterMark);
        serviceProvider.GetRequiredService<HookManager>();
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just toggle the display status of our main ui
        ToggleMainUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => MainWindow.Toggle();
}
