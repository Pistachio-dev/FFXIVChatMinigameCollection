using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.Logging;
using ECommons.GameHelpers;
using MinigameCollection.Save;
using Model.Base;

namespace MinigameCollection
{
    public class PlayerManager
    {
        private readonly PlayerSet players;
        private readonly ILogService logService;
        private readonly ITargetManager targetManager;
        private readonly IClientChatGui chatGui;
        private readonly IChatOutput chatOutput;
        private readonly SaveManager saveManager;

        public PlayerManager(PlayerSet players, ILogService logService, ITargetManager targetManager,
            IClientChatGui chatGui, IChatOutput chatOutput, SaveManager saveManager)
        {
            this.players = players;
            this.logService = logService;
            this.targetManager = targetManager;
            this.chatGui = chatGui;
            this.chatOutput = chatOutput;
            this.saveManager = saveManager;
        }

        public MGPlayer GetPlayer(string fullName, bool muteLog = false)
        {
            if (!muteLog) logService.Debug($"Getting player {fullName}");
            var existing = players.GetPlayer(fullName);
            if (existing != null)
            {
                if (!muteLog) logService.Debug("Success");
                return existing;
            }

            if (!muteLog) logService.Debug("Not found");
            return null;
        }

        public bool TryAddTargetedPlayer()
        {
            if (targetManager.Target is IPlayerCharacter target)
            {
                string playerName = target.GetNameWithWorld();
                AddPlayer(playerName);
                saveManager.Save();
                return true;
            }

            string msg = "Can't add target: not targeting a player.";
            logService.Info(msg);
            chatGui.Print(msg);
            return false;
        }

        public void AddPlayer(string fullName)
        {
            var created = players.AddPlayer(fullName);
            if (created)
            {
                logService.Info($"Created player {fullName}");
                chatOutput.WriteChat($"{fullName} joins the game.");
                saveManager.Save();
                return;
            }

            chatOutput.WriteChat($"Could not add {fullName}. Maybe they're already in.");
            logService.Info($"Could not create player {fullName}");
        }

        public void TogglePlayerAFK(MGPlayer player)
        {
            Plugin.Log.Info($"[ACTION] Toggle AFK. Player: {player.FullName}.");
            player.Afk = !player.Afk;
            chatOutput.WriteChat($"{player.FullName} is {(player.Afk ? "AFK" : "no longer AFK")}");
            saveManager.Save();
        }

        public void ChatSoundWakeUp(MGPlayer player)
        {
            Plugin.Log.Info($"[ACTION] Wake up through chat sound. Player: {player.FullName}.");
            chatOutput.WriteChat($"{player.FullName} it's your turn! <se.9");
        }

        public void Remove(MGPlayer player)
        {
            chatOutput.WriteChat($"{player.FullName} leaves the game.");            
            players.Remove(player);
            saveManager.Save();
        }
    }
}
