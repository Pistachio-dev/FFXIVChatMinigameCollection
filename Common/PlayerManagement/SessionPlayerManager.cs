using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using DalamudBasics.Logging;
using DalamudBasics.Targeting;
using Model.PlayerManagement;
using System;


namespace MinigameCollection.Common.GameBoardCommon
{
    public class SessionPlayerManager<T> : ISessionPlayerManager<T> where T: IGameSpecificPlayerData
    {
        private readonly ITargetingService targeting;
        private readonly ILogService logService;
        private readonly IClientChatGui chatGui;
        private readonly IClientState clientState;

        private readonly IChatOutput chatOutput;
        private readonly SessionPlayers<T> players;
        private readonly IOOGPlayerManager oogPlayerService;

        public SessionPlayerManager(ITargetingService targeting, ILogService logService, IClientChatGui chatGui, IClientState clientState,
            IChatOutput chatOutput, IGameInstance<T> gameInstance, IOOGPlayerManager playerManager)
        {
            this.targeting = targeting;
            this.logService = logService;
            this.chatGui = chatGui;
            this.clientState = clientState;

            this.chatOutput = chatOutput;
            this.players = gameInstance.Players;
            this.oogPlayerService = playerManager;
        }

        public PlayerInSession<T>? GetOrAddHostPlayer()
        {
            if (clientState?.LocalPlayer == null)
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

        public PlayerInSession<T>? AddPlayer(string fullName, bool asCroupier = false)
        {
            if (!ValidateTargetFullName(fullName))
            {
                return null;
            }

            var existingPlayer = oogPlayerService.GetPlayerWithCashRecord(fullName);
            if (existingPlayer == null)
            {
                logService.Info($"Player {fullName} is not in database. Creating it.");
                existingPlayer = oogPlayerService.CreatePlayer(fullName.GetNameOnly(), fullName.GetWorld());
                return null;
            }

            PlayerInSession<T> playerInSession = new PlayerInSession<T>(existingPlayer);
            if (asCroupier)
            {
                players.Dealer = playerInSession;
                chatOutput.WriteChat($"{fullName} joins as dealer");
            }
            else
            {
                players.Spectating.Add(playerInSession);
                chatOutput.WriteChat($"{fullName} joins as spectator");
            }

            return playerInSession;
        }


        public PlayerInSession<T>? AddTargetPlayer()
        {
            string fullName = targeting.GetTargetName();
            return AddPlayer(fullName);
        }

        public void RemovePlayer(string fullName)
        {
            if (players.RemovePlayer(fullName))
            {
                chatOutput.WriteChat($"{fullName} leaves.");
            }
            else
            {
                chatGui.PrintError("That player does not exist in this game instance");
            }
        }

        public bool IsPlayerInSession(string fullName)
        {
            return players.IsPlayerInSession(fullName.GetNameOnly(), fullName.GetWorld());
        }

        public bool TogglePlayerAsAFK(string fullName)
        {
            PlayerInSession<T>? player = players.GetPlayer(fullName);

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

            if (IsPlayerInSession(nameWithWorld))
            {
                logService.Warning("Could not add target player: already in session");
                chatGui.PrintError("Player is already in game");
                return false;
            }

            return true;
        }

        public bool MakePlayerActive(string fullName)
        {
            return players.MoveToActivePlayer(fullName);
        }

        public bool MakePlayerSpectator(string fullName)
        {
            return players.MoveToSpectator(fullName);
        }

        public PlayerInSession<T>? GetPlayer(string fullName)
        {
            return players.GetPlayer(fullName);
        }

        public PlayerInSession<T>? GetDealer()
        {
            return players.Dealer;
        }

        public bool IsPlayerInSession(string name, string world)
        {
            return players.IsPlayerInSession(name, world);
        }
    }
}
