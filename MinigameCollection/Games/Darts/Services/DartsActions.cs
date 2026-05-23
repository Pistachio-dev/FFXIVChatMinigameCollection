using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using MinigameCollection.Games.Common;
using MinigameCollection.Save;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsActions
    {
        private readonly GameHost gameHost;
        private readonly DartsGameState gameState;
        private readonly RollTracker rollTracker;
        private readonly DartsChatOutput chatOutput;
        private readonly BankActions bank;
        private readonly SaveManager saveManager;
        private readonly CommonActions commonActions;
        private readonly Configuration config;
        private const int OrderRollMax = 100;



        public DartsActions(GameHost gameHost, DartsGameState gameState, RollTracker rollTracker,
            IConfigurationService<Configuration> config, DartsChatOutput chatOutput, BankActions bank, SaveManager saveManager, CommonActions commonActions)
        {
            this.gameHost = gameHost;
            this.gameState = gameState;
            this.rollTracker = rollTracker;
            this.config = config.GetConfiguration();
            this.chatOutput = chatOutput;
            this.bank = bank;
            this.saveManager = saveManager;
            this.commonActions = commonActions;
        }
        public void StartOrderRound()
        {
            if (gameHost.Players.ActivePlayers.Any(p => p.Bank.Stored < gameState.Bet))
            {
                gameHost.ChatGui.PrintError("One or more players can't afford that bet");
                return;
            }

            foreach (var player in gameHost.Players.ActivePlayers)
            {
                Plugin.Log.Warning($"Enqueing for {player.FullName}");
                commonActions.SetupRoll(AcceptedRollType.Any, OrderRollMax, (roll) => SetPlayerOrderRoll(player, roll.RollResult), null);
            }
        }

        public void ProcessRoll(DiceRoll roll)
        {
            rollTracker.ProcessRoll(roll);
        }

        // Called on detecting a /random or /dice
        private void SetPlayerOrderRoll(MGPlayer? player, int order)
        {
            if (player == null)
            {
                Plugin.Log.Error("Attempting to set player order roll, but player is null");
                return;
            }
            var data = player.GetData<DartsPlayerData>(DartsGame.Id);
            data.OrderRolled = order;
            player.SetData<DartsPlayerData>(DartsGame.Id, data);

            if (gameHost.Players.ActivePlayers.All(p => p.GetData().OrderRolled != -1))
            {
                ShufflePlayersBasedOnRolledOrder();
                chatOutput.WritePlayerOrder(gameHost.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).ToList());
                StartGame();
            }
        }

        private void ShufflePlayersBasedOnRolledOrder()
        {
            var ordered = gameHost.Players.Reorder(p => p.GetData().OrderRolled);
            Plugin.Log.Info($"New player order: {gameHost.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).Humanize()}");
        }

        private void StartGame()
        {
            var currentPlayer = gameHost.Players.ActivePlayers.FirstOrDefault();
            if (currentPlayer == null)
            {
                Plugin.Log.Error("Attempting to start game, but no current player found");
                return;
            }
            chatOutput.RequestThrow(currentPlayer);
        }
    }
}
