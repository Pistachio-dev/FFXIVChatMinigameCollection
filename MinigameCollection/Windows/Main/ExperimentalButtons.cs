using Dalamud.Interface.ImGuiNotification;
using ImGuiNET;
using Lumina.Excel.Sheets;
using System.IO;
using System.Linq;
using System.Text;

namespace MinigameCollection.Windows.Main
{
    public partial class MainWindow
    {
        private void DrawExperimentalButtons()
        {
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
                foreach (var obj in objectTable)
                {

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
    }
}
