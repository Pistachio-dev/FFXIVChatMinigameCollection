using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
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
        private readonly GRChatOutput chatOutput;
        private const int RevolverRollMax = 7;
        private const int OrderRollMax = 100;
        public GRActions(GameHost gameHost, GRGameState gameState, RollTracker rollTracker, IConfigurationService<Configuration> config, GRChatOutput chatOutput)
        {
            this.gameHost = gameHost;
            this.gameState = gameState;
            this.rollTracker = rollTracker;
            this.config = config.GetConfiguration();
            this.chatOutput = chatOutput;
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

        public void FinishOrderAndStartShooting()
        {
            Plugin.Log.Info("Ending roll order phase");
            ShufflePlayersBasedOnRolledOrder();
            Plugin.Log.Info("Starting shooting phase");
            gameState.Stage = GRStage.Shooting;
            gameState.CurrentPlayer = gameHost.Players.Players.FirstOrDefault() ?? throw new Exception("Attempting to start shooting, but there are no players");
            SetupCurrentPlayerRoll();
        }

        public void SetupCurrentPlayerRoll()
        {
            var player = gameState.CurrentPlayer ?? throw new Exception("Trying to set up current player roll to be awaited, but current player is null");
            gameHost.ChatOutput.WriteChat($"{gameState.CurrentPlayer?.FullName}'s turn. /dice 7, please.");
            rollTracker.QueueExpectedRoll(player.FullName, config.AcceptedRollType, RevolverRollMax, ProcessShotRoll);
        }

        private void ProcessShotRoll(DiceRoll role)
        {
            if (gameState.ChambersLoaded.Contains(role.RollResult))
            {
                chatOutput.DrawPlayerShot(gameState.CurrentPlayer);
                var pData = gameState.CurrentPlayer?.GetData() ?? throw new Exception("Processing shot roll, but current player is null");
                pData.Alive = false;
                gameState.CurrentPlayer.SetData(pData);
                if (gameState.WinCondition())
                {
                    var winner = gameHost.Players.Players.FirstOrDefault(p => p.GetData().Alive) ?? throw new Exception("Garlean Roulette ended with no winners. This is not supposed to happen");
                    chatOutput.WriteWinner(gameState.CurrentPlayer);
                    gameState.Stage = GRStage.Winner;
                }
                gameState.CurrentPlayer = gameHost.Players.GetNext(gameState.CurrentPlayer, p => p.GetData().Alive);
                SetupCurrentPlayerRoll();
            }
            else
            {
                chatOutput.DrawPlayerSurvives(gameState.CurrentPlayer);
            }

            
        }


        private void ShufflePlayersBasedOnRolledOrder()
        {
            var ordered = gameHost.Players.Reorder(p => p.GetData().OrderRolled);
            Plugin.Log.Verbose($"New player order: {gameHost.Players.Players.Select(p => p.FullName.GetFirstName()).Humanize()}");
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
