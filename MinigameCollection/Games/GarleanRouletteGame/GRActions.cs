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
        private const int RevolverRollMin = 1;
        private const int RevolverRollMaxInclusive = 7;
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

            if (gameHost.Players.Players.Count < 2)
            {
                gameHost.ChatGui.PrintError("Not enough players. Needs two at least");
                return;
            }

            gameState.Stage = GRStage.RollingOrder;
            gameHost.ChatOutput.WriteChat("Rolling player order");

            foreach (var player in gameHost.Players.Players)
            {
                var data = player.GetData();
                data.Reset();
                player.SetData(data);

                rollTracker.QueueExpectedRoll(gameHost.GetHostPlayerFullName(), config.AcceptedRollType, OrderRollMax,   (roll) => SetPlayerOrderRoll(player, roll.RollResult));
                gameHost.ChatOutput.WriteChat($"{player.FullName.GetFirstName()}:", minSpacingBeforeInMs: 1500);
                gameHost.ChatOutput.WriteDiceCommand(100, config.DefaultOutputChatType == Dalamud.Game.Text.XivChatType.Alliance);
            }
        }

        private void SetPlayerOrderRoll(MGPlayer player, int order)
        {
            var data = player.GetData();
            data.OrderRolled = order;
            player.SetData(data);
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
            AddBullet(true);
            SetupCurrentPlayerRoll();
        }

        public void RollInsteadOfPlayer()
        {
            rollTracker.AcceptNextRollRegardless();
            chatOutput.RollDiceAsHouse(gameState.CurrentPlayer.FullName, RevolverRollMaxInclusive, config.DefaultOutputChatType == Dalamud.Game.Text.XivChatType.Alliance);
        }

        public void SetupCurrentPlayerRoll(bool isHousePressingTheTrigger = false)
        {
            var player = gameState.CurrentPlayer ?? throw new Exception("Trying to set up current player roll to be awaited, but current player is null");
            gameHost.ChatOutput.WriteChat($"{gameState.CurrentPlayer?.FullName}'s turn. /dice 7, please. <se.3>", minSpacingBeforeInMs: 1000);
            rollTracker.QueueExpectedRoll(player.FullName, config.AcceptedRollType, RevolverRollMaxInclusive, ProcessShootRoll);
        }

        private void ProcessShootRoll(DiceRoll role)
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
                    chatOutput.WriteWinner(winner);
                    gameState.Stage = GRStage.Winner;
                    return;
                }
            }
            else
            {
                chatOutput.DrawPlayerSurvives(gameState.CurrentPlayer);
            }

            gameState.TriggerPulls++;
            if (gameState.TriggerPulls == gameHost.Players.GetNonAfkPlayers().Count())
            {
                AddBullet(false);
            }
            gameState.CurrentPlayer = gameHost.Players.GetNext(gameState.CurrentPlayer, p => p.GetData().Alive);
            Plugin.Log.Verbose("Setting next player: " + gameState.CurrentPlayer.FullName);
            SetupCurrentPlayerRoll();
        }

        private void AddBullet(bool isFirstTime)
        {
            if (!isFirstTime)
            {
                gameHost.ChatOutput.WriteChat("Everybody has survived so far... Let's up the stakes", minSpacingBeforeInMs: 2000);
            }
            gameState.TriggerPulls = 0;
            if (gameState.ChambersLoaded.Count == RevolverRollMaxInclusive)
            {
                gameHost.ChatOutput.WriteChat("All chambers are loaded! How lucky can you get?");
                return;
            }

            bool bulletInserted = false;
            while (!bulletInserted)
            {
                var bullet = new Random().Next(RevolverRollMin, RevolverRollMaxInclusive + 1);
                if (!gameState.ChambersLoaded.Contains(bullet))
                {
                    gameState.ChambersLoaded.Add(bullet);
                    gameHost.ChatOutput.WriteChat($"Inserting a new bullet on chamber {bullet}");
                    gameHost.ChatOutput.WriteChat($"The chambers with bullets are now: {gameState.ChambersLoaded.Humanize()}", minSpacingBeforeInMs: 2000);
                    gameHost.ChatOutput.WriteChat($"The host spins the drum.");
                    bulletInserted = true;
                }
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
