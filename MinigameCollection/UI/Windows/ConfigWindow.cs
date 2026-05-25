using Dalamud.Bindings.ImGui;
using Dalamud.Game.Text;
using DalamudBasics.Configuration;
using DalamudBasics.GUI.Forms;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Games.Darts.Services;
using MinigameCollection.Games.GarleanRouletteGame;
using System;
using System.Linq;

namespace MinigameCollection.UI.Windows;

public class ConfigWindow : PluginWindowBase, IDisposable
{
    private IConfigurationService<Configuration> configService;
    private Configuration configuration;
    private ImGuiFormFactory<Configuration> formFactory;
    private GameHost host;

    public ConfigWindow(ILogService logService, IServiceProvider sp) : base(logService, "Configuration##MinigameCollectionConfiguration")
    {
        configService = sp.GetRequiredService<IConfigurationService<Configuration>>();
        configuration = configService.GetConfiguration();
        formFactory = new ImGuiFormFactory<Configuration>(() => configuration, (data) => Save());
        host = sp.GetRequiredService<GameHost>();
    }

    public void Dispose()
    { }

    public override void PreDraw()
    {
    }

    protected override void SafeDraw()
    {
        DrawChatChannelSelector();
    }

    private void DrawChatChannelSelector()
    {
        XivChatType[] channels = { XivChatType.Echo, XivChatType.Party, XivChatType.Alliance, XivChatType.Say, XivChatType.Yell, XivChatType.Shout,
                XivChatType.Ls1, XivChatType.Ls2, XivChatType.Ls3, XivChatType.Ls4, XivChatType.Ls5, XivChatType.Ls6, XivChatType.Ls7, XivChatType.Ls8,
                XivChatType.CrossLinkShell1, XivChatType.CrossLinkShell2, XivChatType.CrossLinkShell3, XivChatType.CrossLinkShell4, XivChatType.CrossLinkShell5, XivChatType.CrossLinkShell6, XivChatType.CrossLinkShell7, XivChatType.CrossLinkShell8};
        string[] options = channels.Select(entry => entry.ToString()).ToArray();
        var local = channels.IndexOf(configService.GetConfiguration().DefaultOutputChatType);
        if (local == -1)
        {
            local = 0;
        }
        if (ImGui.Combo("Chat channel", ref local, options))
        {
            if (local >= channels.Length)
            {
                return;
            }
            configService.GetConfiguration().DefaultOutputChatType = channels[local];
            configService.SaveConfiguration();
        }

        (var selectedGameId, _, _) = host.AvailableGames()[configuration.SelectedGame];
        if (selectedGameId == GarleanRoulette.Id)
        {
            formFactory.DrawCheckbox("Start from first player if gun empties", nameof(Configuration.GarleanRouletteRestartIfGunEmpties));
        }
        if (selectedGameId == DartsGame.Id)
        {
            formFactory.DrawCheckbox("Fail if going over score", nameof(Configuration.DartsNeedExactThrow));
            formFactory.DrawIntInput("Target score", nameof(Configuration.DartsTargetScore));
            formFactory.DrawIntInput("Darts amount per turn", nameof(Configuration.DartsAmountPerTurn));
        }
    }

    private void Save()
    {
        configService.SaveConfiguration();
        logService.Info("Configuration saved.");
    }


}
