using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using MinigameCollection.Dice;
using MinigameCollection.Games.Common;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal partial class DartsActions
    {
        public void StartOrderRound()
        {
            gameState.Stage = DartsStage.RollingOrder;
            if (gameHost.Players.ActivePlayers.Any(p => p.Bank.Stored < gameState.Bet))
            {
                gameHost.ChatGui.PrintError("One or more players can't afford that bet");
                return;
            }

            foreach (var player in gameHost.Players.ActivePlayers)
            {
                Plugin.Log.Warning($"Enqueing for {player.FullName}");
                commonActions.SetupRoll(AcceptedRollType.Any, OrderRollMax, (roll) => OnOrderRollDetected(player, roll.RollResult), true, null);
            }
        }

        public void ProcessRoll(DiceRoll roll)
        {
            rollTracker.ProcessRoll(roll);
        }

        private void OnOrderRollDetected(MGPlayer? player, int order)
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
                FinishOrderingAndStartGame();
            }
        }

        private void FinishOrderingAndStartGame()
        {

            ShufflePlayersBasedOnRolledOrder();
            chatOutput.WritePlayerOrder(gameHost.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).ToList());
            StartGame();
        }

    }
}
