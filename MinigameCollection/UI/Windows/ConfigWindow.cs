using Dalamud.Bindings.ImGui;
using DalamudBasics.Configuration;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Numerics;

namespace MinigameCollection.UI.Windows;

public class ConfigWindow : PluginWindowBase, IDisposable
{
    private IConfiguration configuration;

    public ConfigWindow(ILogService logService, IServiceProvider sp) : base(logService, "Configuration")
    {
        Size = new Vector2(232, 90);
        SizeCondition = ImGuiCond.Always;

        configuration = sp.GetRequiredService<IConfiguration>();
    }

    public void Dispose()
    { }

    public override void PreDraw()
    {
    }

    protected override void SafeDraw()
    {
    }
}
