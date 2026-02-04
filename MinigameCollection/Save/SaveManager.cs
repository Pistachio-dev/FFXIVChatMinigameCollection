using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MinigameCollection.Save
{
    public class SaveManager
    {
        private readonly PlayerSet playerData;
        private readonly IFramework framework;

        private string Route {  get; set; }
        public SaveManager(IDalamudPluginInterface pluginInterface, PlayerSet playerData, IFramework framework)
        {
            Route = Path.Combine(pluginInterface.ConfigDirectory.FullName, "playersSave.json");
            this.playerData = playerData;
            this.framework = framework;
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize<PlayerSet>(this.playerData);
            framework.RunOnFrameworkThread(() =>
            {
                File.WriteAllText(Route, json);
                Plugin.Log.Info("Save written");
            });
            
        }

        public void Load()
        {
            if (!File.Exists(Route))
            {
                Plugin.Log.Warning("Skipping load: file does not yet exist");
                return;
            }
            var json = File.ReadAllText(Route);
            var playerSet = JsonSerializer.Deserialize<PlayerSet>(json);
            if (playerSet != null)
            {
                playerData.Restore(playerSet);
            }            
        }
    }
}
