using DalamudBasics.Chat.Output;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Dice;
using Model.Base;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRActions
    {
        private readonly GameHost gameHost;
        private readonly GRGameState gameState;
        private readonly RollTracker rollTracker;
        private readonly Configuration config;
        private const int RevolverRollMax = 7;
        private const int OrderRollMax = 100;
        public GRActions(GameHost gameHost, GRGameState gameState, RollTracker rollTracker, Configuration config)
        {
            this.gameHost = gameHost;
            this.gameState = gameState;
            this.rollTracker = rollTracker;
            this.config = config;
        }

        public void StartOrderRound()
        {
            gameState.Stage = GRStage.RollingOrder;
            gameHost.ChatOutput.WriteChat("Rolling player order");
            if (gameHost.Players.Players.Count < 2)
            {
                gameHost.ChatGui.PrintError("Not enough players. Needs two at least");
            }
            foreach (var player in gameHost.Players.Players)
            {
                var data = player.GetData();
                data.OrderRolled = -1;
                player.SetData(data);

                rollTracker.QueueExpectedRoll(gameHost.GetHostPlayerFullName(), config.AcceptedRollType, OrderRollMax,   (roll) => SetPlayerOrderRoll(player, roll.RollResult));
                gameHost.ChatOutput.WriteChat($"{player.FullName.GetFirstName()}:", minSpacingBeforeInMs: 1500);
                gameHost.ChatOutput.WriteCommand("/dice 100");
            }
        }

        private void SetPlayerOrderRoll(MGPlayer player, int order)
        {
            var data = player.GetData();
            data.OrderRolled = order;
            player.SetData(data);
        }
        public void CastRoll(string playerFullName)
        {
            if (!MakeSureCurrentPlayerExists())
            {
                Plugin.Log.Warning("Could not \"Cast roll\", player list is empty");
                return;
            }

            gameState.CurrentPlayer = gameHost.Players.GetNext(gameState.CurrentPlayer);
            gameHost.ChatOutput.WriteCommand("/dice");
        }

        public void ProcessRoll(DiceRoll roll)
        {
            rollTracker.ProcessRoll(roll);            
        }

        public void OnWin()
        {
            MGPlayer? survivor = gameHost.Players.Players.FirstOrDefault(p => p.GetData().Alive);
            if (survivor == null)
            {
                Plugin.Log.Warning("No survivors at the end of game. This should not happen.");
            }

            Plugin.Log.Warning($"{survivor.FullName} wins!");
        }

        private void FinishRollOrderStage()
        {
            // All orders have been rolled
            gameHost.Players.Reorder(p => p.GetData<GRPlayerData>(GarleanRoulette.Id).OrderRolled);
            var order = gameHost.Players.Players.Select(x => x.FullName.GetFirstName()).Humanize();
            gameHost.ChatOutput.WriteChat("Order: " + order);
            gameState.CurrentPlayer = gameHost.Players.GetFirst();
            gameState.Stage = GRStage.Shooting;
            PlayerTurn();
        }

        private void PlayerTurn()
        {
            gameHost.ChatOutput.WriteChat($"{gameState.CurrentPlayer?.FullName}'s turn.");
        }
        private bool MakeSureCurrentPlayerExists()
        {
            if (gameState.CurrentPlayer != null)
            {
                return true;
            }

            var first = gameHost.Players.GetFirst();
            if  (first == null)
            {
                return false;
            }

            gameState.CurrentPlayer = first;

            return true;
        }
    }
}
