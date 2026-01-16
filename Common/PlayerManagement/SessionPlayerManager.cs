using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using DalamudBasics.Logging;
using DalamudBasics.Targeting;
using ECommons.GameHelpers;
using Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using System.Runtime.CompilerServices;

namespace MinigameCollection.Common.GameBoardCommon
{
    public class SessionPlayerManager : ISessionPlayerManager
    {
        private readonly ITargetingService targeting;
        private readonly ILogService logService;
        private readonly IClientChatGui chatGui;
        private readonly IClientState clientState;

        private readonly IChatOutput chatOutput;
        private readonly IGameInstance gameInstance;
        private readonly IPlayerRepository playerRepo;

        public SessionPlayerManager(ITargetingService targeting, ILogService logService, IClientChatGui chatGui, IClientState clientState,
            IChatOutput chatOutput, IGameInstance gameInstance, IPlayerRepository playerRepo)
        {
            this.targeting = targeting;
            this.logService = logService;
            this.chatGui = chatGui;
            this.clientState = clientState;

            this.chatOutput = chatOutput;
            this.gameInstance = gameInstance;
            this.playerRepo = playerRepo;
        }

        public PlayerInSession? GetOrAddHostPlayer()
        {
            if (clientState.LocalPlayer == null)
            {
                return null;
            }
            string hostName = clientState.LocalPlayer.GetFullName();
            var hostInSession = GetPlayer(hostName);
            if (hostInSession != null)
            {
                return hostInSession;
            }

            return AddPlayer(hostName, true);
        }

        public PlayerInSession? AddPlayer(string fullName, bool asCroupier = false)
        {
            if (!ValidateTargetFullName(fullName))
            {
                return null;
            }

            var existingPlayer = playerRepo.GetPlayerWithCashRecord(fullName);
            if (existingPlayer == null)
            {
                logService.Info($"Player {fullName} is not in database. Creating it.");
                var player = new PlayerOOGData(fullName.GetWorld(), fullName.GetWorld());
                if (!playerRepo.CreatePlayer(player))
                {
                    logService.Error("Could not create new player and it does not exist. Can't add player to game");
                    return null;
                }

                logService.Info($"Player {fullName} created.");
                existingPlayer = playerRepo.GetPlayerWithCashRecord(fullName);
            }

            var playerInSession = new PlayerInSession(existingPlayer);
            if (asCroupier)
            {
                gameInstance.Players.Dealer = playerInSession;
                chatOutput.WriteChat($"{fullName} joins as dealer");

            }
            else
            {
                gameInstance.Players.Spectating.Add(playerInSession);
                chatOutput.WriteChat($"{fullName} joins as spectator");
            }
¡
            return playerInSession;
        }


        public PlayerInSession? AddTargetPlayer()
        {
            string fullName = targeting.GetTargetName();
            return AddPlayer(fullName);
        }

        public void RemovePlayer(string fullName)
        {
            if (gameInstance.Players.RemovePlayer(fullName))
            {
                chatOutput.WriteChat($"{fullName} leaves.");
            }
            else
            {
                chatGui.PrintError("That player does not exist in this game instance");
            }
        }

        public bool IsPlayerInSession(string name, string world)
        {
            return gameInstance.Players.IsPlayerInSession(name, world);
        }

        public bool TogglePlayerAsAFK(string fullName)
        {
            var player = gameInstance.Players.GetPlayer(fullName);

            if (player == null)
            {
                return false;
            }

            player.IsAFK = !player.IsAFK;

            return true;
        }

        private bool ValidateTargetFullName(string nameWithWorld)
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

        public bool MakePlayerActive(string fullName)
        {
            return gameInstance.Players.MoveToActivePlayer(fullName);
        }

        public bool MakePlayerSpectator(string fullName)
        {
            return gameInstance.Players.MoveToSpectator(fullName);
        }

        public PlayerInSession? GetPlayer(string fullName)
        {
            return gameInstance.Players.GetPlayer(fullName);
        }

        public PlayerInSession? GetDealer()
        {
            return gameInstance.Players.Dealer;
        }
    }
}
