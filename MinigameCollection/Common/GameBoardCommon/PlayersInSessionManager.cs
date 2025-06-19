using Dalamud.Plugin.Services;
using Dalamud.Utility;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Logging;
using DalamudBasics.Targeting;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.GameBoardCommon
{
    public class PlayersInSessionManager
    {
        private readonly ITargetingService targeting;
        private readonly ILogService logService;
        private readonly IClientChatGui chatGui;
        private readonly IPlayerOOGDataRepository playerOOGRepo;
        private readonly IPlayerIdentifierRepository playerIdentifierRepo;

        public PlayersInSessionManager(ITargetingService targeting, ILogService logService, IClientChatGui chatGui,
            IPlayerOOGDataRepository playerOOGRepo, IPlayerIdentifierRepository playerIdentifierRepo)
        {
            this.targeting = targeting;
            this.logService = logService;
            this.chatGui = chatGui;
            this.playerOOGRepo = playerOOGRepo;
            this.playerIdentifierRepo = playerIdentifierRepo;
        }

        public List<PlayerInSession> InGame { get; set; } = new();

        public void AddTargetPlayer()
        {
            string nameWithWorld = targeting.GetTargetName();
            if (!ValidateFullName(nameWithWorld))
            {
                return;
            }

            string[] splitName = nameWithWorld.Split('@');

            var existingPlayer = playerOOGRepo.FindOne(p => p.Is(splitName[0], splitName[1]));
            if (existingPlayer == null)
            {
                existingPlayer = new PlayerOOGData(splitName[0], splitName[1]);
            }

            InGame.Add(new PlayerInSession(existingPlayer));
        }

        public bool IsPlayerInSession(string name, string world)
        {
            return InGame.Any(p => p.Is(name, world));// || Spectating.Any(p => p.Is(name, world));
        }

        private bool ValidateFullName(string nameWithWorld)
        {            
            if (nameWithWorld.IsNullOrEmpty())
            {
                logService.Warning("Could not add target player: retrieved name is empty.");
                chatGui.PrintError("Not targeting a player");
                return false; ;
            }

            var splitName = nameWithWorld.Split("@");
            if (splitName.Length < 2)
            {
                logService.Warning("Could not add target player: no world name retrieved.");
                chatGui.PrintError("Error trying to add player, check /xllogs");
                return false;
            }

            string name = splitName[0];
            string world = splitName[1];

            if (IsPlayerInSession(name, world))
            {
                logService.Warning("Could not add target player: already in session");
                chatGui.PrintError("Player is already in game");
                return false;
            }

            return true;
        }
    }
}
