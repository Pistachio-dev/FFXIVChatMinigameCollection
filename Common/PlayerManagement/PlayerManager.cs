using Common.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using DalamudBasics.Logging;
using DalamudBasics.Targeting;
using Model.PlayerManagement;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.GameBoardCommon
{
    public class PlayerManager : IPlayerManager
    {
        private readonly ITargetingService targeting;
        private readonly ILogService logService;
        private readonly IClientChatGui chatGui;
        private readonly IClientState clientState;

        private readonly IChatOutput chatOutput;

        public PlayerManager(ITargetingService targeting, ILogService logService, IClientChatGui chatGui,IClientState clientState,
            IChatOutput chatOutput)
        {
            this.targeting = targeting;
            this.logService = logService;
            this.chatGui = chatGui;
            this.clientState = clientState;

            this.chatOutput = chatOutput;
        }

        public List<PlayerInSession> InGame { get; } = new();

        public PlayerInSession? AddPlayer(string fullName)
        {
            if (!ValidateFullName(fullName))
            {
                return null;
            }

            //string[] splitName = fullName.Split('@');

            //var existingPlayer = playerOOGRepo.GetPlayerOrDefault(splitName[0], splitName[1]);
            //if (existingPlayer == null)
            //{
            //    existingPlayer = new PlayerOOGDataEntity(splitName[0], splitName[1]);
            //    logService.Info($"No DB info for player {existingPlayer.FullName}. Adding it.");
            //    playerOOGRepo.Add(existingPlayer);
            //    //TODO: Add a financial record at this point too
            //}

            //var playerAdded = new PlayerInSession(existingPlayer);
            //InGame.Add(playerAdded);
            //chatOutput.WriteChat($"{existingPlayer.FullName} joins the game.");

            //return playerAdded;
            throw new NotImplementedException();
        }

        public PlayerInSession? AddTargetPlayer()
        {
            string fullName = targeting.GetTargetName();
            return AddPlayer(fullName);
        }

        public void RemovePlayer(string fullName)
        {
            var player = InGame.FirstOrDefault(p => p.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase));
            if (player == null)
            {
                return;
            }

            InGame.Remove(player);
        }

        public bool IsPlayerInSession(string name, string world)
        {
            return InGame.Any(p => p.Is(name, world));// || Spectating.Any(p => p.Is(name, world));
        }

        public bool TogglePlayerAsAFK(string name, string world)
        {
            var player = InGame.FirstOrDefault(p => p.PlayerOOGData.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                && p.PlayerOOGData.World.Equals(world, StringComparison.CurrentCultureIgnoreCase));

            if (player == null)
            {
                return false;
            }

            player.IsAFK = !player.IsAFK;

            return true;
        }

        private bool ValidateFullName(string nameWithWorld)
        {            
            if (nameWithWorld.IsNullOrEmpty())
            {
                logService.Warning("Could not add target player: retrieved name is empty.");
                chatGui.PrintError("Not targeting a player");
                return false;
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

        public PlayerOOGData? GetPlayer(string fullName)
        {
            throw new NotImplementedException();
            //string[] split = fullName.Split('@');
            //if (split.Length < 2)
            //{
            //    return null;
            //}
            //return playerOOGRepo.GetPlayerOrDefault(split[0], split[1]);
        }

        private PlayerOOGData dealer;
        public PlayerOOGData GetDealer()
        {
            throw new NotImplementedException();
            //var localPlayer = clientState.LocalPlayer;
            //if (localPlayer == null)
            //{
            //    chatGui.PrintError("No local player");
            //    throw new Exception("No local player");
            //}

            //var name = localPlayer.GetFullName();
            //if (dealer == null || dealer.Name != name)
            //{
            //    var split = name.Split('@');
            //    var dealerInDB = playerOOGRepo.GetPlayerOrDefault(split[0], split[1]);
            //    if (dealerInDB == null)
            //    {
            //        dealerInDB = new PlayerOOGData(split[0], split[1]);
            //        playerOOGRepo.Add(dealerInDB);
            //    }

            //    dealer = dealerInDB;
            //}

            //return dealer;
        }
    }
}
