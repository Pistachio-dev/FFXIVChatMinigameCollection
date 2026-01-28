using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Logging;
using DalamudBasics.Targeting;
using ECommons.GameHelpers;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MinigameCollection
{
    public class PlayerManager
    {
        private readonly PlayerSet players;
        private readonly ILogService logService;
        private readonly ITargetManager targetManager;
        private readonly IClientChatGui chatGui;

        public PlayerManager(PlayerSet players, ILogService logService, ITargetManager targetManager, IClientChatGui chatGui)
        {
            this.players = players;
            this.logService = logService;
            this.targetManager = targetManager;
            this.chatGui = chatGui;
        }

        public MGPlayer GetPlayer(string fullName)
        {
            logService.Debug($"Getting player {fullName}");
            var existing = players.GetPlayer(fullName);
            if (existing != null)
            {
                logService.Debug("Success");
                return existing;    
            }

            logService.Debug("Not found");
            return null;
        }

        public bool TryAddTargetedPlayer()
        {
            if (targetManager.Target is IPlayerCharacter target)
            {
                string playerName = target.GetNameWithWorld();
                AddPlayer(playerName);
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
                return;
            }

            logService.Info($"Could not create player {fullName}");
        }
    }
}
