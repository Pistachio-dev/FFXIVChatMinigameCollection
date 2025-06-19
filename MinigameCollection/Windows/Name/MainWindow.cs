using Dalamud.Interface.ImGuiNotification;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.GUI.Windows;
using DalamudBasics.Logging;
using FFXIVClientStructs.FFXIV.Client.Game.Fate;
using ImGuiNET;
using Lumina.Excel.Sheets;
using Microsoft.Extensions.DependencyInjection;
using MinigameCollection.Common;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MinigameCollection.Windows.Name;

public class MainWindow : PluginWindowBase, IDisposable
{
    private IDataManager dataManager;
    private IChatOutput chatOutput;
    private IObjectTable objectTable;
    private INotificationManager notificationManager;
    private GameModeManager gameModeManager;

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
    }

    public void Dispose() { }

    private string queryedSheetName = "";

    protected override void SafeDraw()
    {
        gameModeManager.GetGame(GameSelected.None).Draw();
        if (ImGui.Button("Get lumina sheets"))
        {
            var data = dataManager.GameData.Excel;
            var sheets = data.SheetNames;
            chatOutput.WriteChat(sheets.Count.ToString(), Dalamud.Game.Text.XivChatType.Echo);
            var orderedSheetNames = sheets.OrderBy(sn => sn).ToList();
            var s = new StringBuilder();
            foreach (var sheetName in orderedSheetNames)
            {
                s.AppendLine(sheetName);
            }

            File.WriteAllText("D:\\Code\\Dalamud\\_ReferencesAndNotes\\LuminaGameDataSheets.txt", s.ToString());
            chatOutput.WriteChat("done", Dalamud.Game.Text.XivChatType.Echo);
        }
        ImGui.InputText("", ref queryedSheetName, 30);
        ImGui.SameLine();
        if (ImGui.Button("Read next row"))
        {
            var fateSheet = dataManager.GetExcelSheet<Fate>();
            chatOutput.WriteChat(fateSheet.Count.ToString());
            for (var i = 1000; i < 1010; i++)
            {
                chatOutput.WriteChat(fateSheet.GetRowAt(i).Name.ToString());
            }
        }

        if (ImGui.Button("List entities"))
        {
            foreach (var obj in objectTable) {
                
                logService.Warning($"Kind: {obj.ObjectKind} Name: {obj.Name})");                
            }
        }
        if (ImGui.Button("Notification"))
        {
            var notification = new Notification { Title = "Test notification yay" };
            notificationManager.AddNotification(notification);
        }

        if (ImGui.Button("List FATEs"))
        {
            ListFates();
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
