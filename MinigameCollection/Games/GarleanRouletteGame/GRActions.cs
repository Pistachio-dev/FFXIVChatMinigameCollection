using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using MinigameCollection.Save;
using Model.Base;
using System;
using System.Linq;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRActions : IDisposable
    {
        private readonly GameHost gameHost;
        private readonly GRGameState gameState;
        private readonly RollTracker rollTracker;
        private readonly Configuration config;
        private readonly GRChatOutput chatOutput;
        private readonly BankActions bank;
        private readonly SaveManager saveManager;
        private const int RevolverRollMin = 1;
        private const int RevolverRollMaxInclusive = 7;
        private const int OrderRollMax = 100;

        public GRActions(GameHost gameHost, GRGameState gameState, RollTracker rollTracker, 
            IConfigurationService<Configuration> config, GRChatOutput chatOutput, BankActions bank, SaveManager saveManager)
        {
            this.gameHost = gameHost;
            this.gameState = gameState;
            this.rollTracker = rollTracker;
            this.config = config.GetConfiguration();
            this.chatOutput = chatOutput;
            this.bank = bank;
            this.saveManager = saveManager;
        }

        public void ResetGame(GameHost host)
        {
            ResetPlayers();
            rollTracker.Reset();
            gameState.TriggerPulls = 0;
            gameState.ChambersLoaded.Clear();
            var firstPlayer = host.Players?.GetFirst();
            if (firstPlayer != null)
            {
                gameState.CurrentPlayer = firstPlayer;
            }
            
            // AddTestPlayers(host);
            gameState.Stage = GRStage.NotStarted;
            Plugin.Log.Info($"{nameof(GarleanRoulette)} initialized.");
        }

        public void GoBackToBetting()
        {
            gameState.Stage = GRStage.NotStarted;
        }

        private void ResetPlayers()
        {
            foreach (var player in gameHost.Players.ActivePlayers)
            {
                // Set up the bets
                var data = player.GetData();
                data.Reset();
                player.SetData(data);
                bank.StoreAll(player);
                bank.Draw(player, gameState.Bet);
            }
        }

        public void StartOrderRound()
        {
            if (gameHost.Players.ActivePlayers.Count < 2)
            {
                gameHost.ChatGui.PrintError("Not enough players. Needs two at least");
                return;
            }

            if (gameHost.Players.ActivePlayers.Any(p => p.Bank.Stored < gameState.Bet))
            {
                gameHost.ChatGui.PrintError("One or more players can't afford that bet");
                return;
            }

            gameState.Stage = GRStage.RollingOrder;
            gameHost.ChatOutput.WriteChat("Rolling player order");

            ResetPlayers();

            foreach (var player in gameHost.Players.ActivePlayers)
            {
                // Prepare the expected roll
                Plugin.Log.Warning("Roll expectation queued: " + player.FullName);
                rollTracker.QueueExpectedRoll("Irrelevant, will match house", config.AcceptedRollType, OrderRollMax, true, 
                (roll) => {
                    gameHost.ChatOutput.WriteChat($"{player.FullName.GetFirstName()} rolled {roll.RollResult}");
                    SetPlayerOrderRoll(player, roll.RollResult);
                });
                gameHost.ChatOutput.WriteDiceCommand(100, config.DefaultOutputChatType == Dalamud.Game.Text.XivChatType.Alliance);
            }

            //saveManager.Save();
        }

        private void SetPlayerOrderRoll(MGPlayer player, int order)
        {
            var data = player.GetData();
            data.OrderRolled = order;
            player.SetData(data);
            //saveManager.Save();
        }

        public void ProcessRoll(DiceRoll roll)
        {
            rollTracker.ProcessRoll(roll);
        }

        public void FinishOrderAndStartShooting()
        {
            Plugin.Log.Info("Ending roll order phase");
            ShufflePlayersBasedOnRolledOrder();
            chatOutput.WritePlayerOrder(gameHost.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).ToList());
            Plugin.Log.Info("Starting shooting phase");
            gameState.Stage = GRStage.Shooting;
            gameState.CurrentPlayer = gameHost.Players.ActivePlayers.FirstOrDefault() ?? throw new Exception("Attempting to start shooting, but there are no players");
            AddBullet(true);
            SetupCurrentPlayerRoll();
        }

        public void DumpStateToLog()
        {
            Plugin.Log.Info("===================");
            Plugin.Log.Info($"Stage: {gameState.Stage}. Trigger pulls: {gameState.TriggerPulls}. CurrentPlayer: {gameState.CurrentPlayer?.FullName}");
            Plugin.Log.Info($"Bet: {gameState.Bet}. Did someone die?: {gameState.DidSomeoneDieThisRound}. Chambers loaded: {gameState.ChambersLoaded.Humanize()}");
            foreach (var player in gameHost.Players.AllPlayers)
            {
                Plugin.Log.Info($"{player.FullName}: rolled order ({player.GetData().OrderRolled}), rolled ({player.GetData().Roll}), alive: {player.GetData().Alive})");
            }
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
            rollTracker.QueueExpectedRoll(player.FullName, config.AcceptedRollType, RevolverRollMaxInclusive, false, ProcessShootRoll);
        }

        private MGPlayer GetNextRoundFirstPlayer()
        {
            return gameHost.Players.ActivePlayers.Where(p => p.GetData().Alive).OrderBy(p => p.GetData().OrderRolled).FirstOrDefault()
               ?? throw new Exception("Could not get player for next round. No active alive player found");
        }


        // Returns true if the shot should cause a skip to the first player
        private void OnPlayerShot(int rollResult)
        {
            chatOutput.WritePlayerShot(gameState.CurrentPlayer);
            gameState.ChambersLoaded = gameState.ChambersLoaded.Where(n => n != rollResult).ToList();
            var pData = gameState.CurrentPlayer?.GetData() ?? throw new Exception("Processing shot roll, but current player is null");
            pData.Alive = false;
            gameState.DidSomeoneDieThisRound = true;
            gameState.CurrentPlayer.SetData(pData);
            if (gameState.WinCondition())
            {
                OnWin();
                return;
            }
            else if (gameState.ChambersLoaded.Count == 0 && gameState.TriggerPulls < gameHost.Players.ActivePlayers.Count() && config.GarleanRouletteRestartIfGunEmpties)
            {
                chatOutput.WriteGunEmptied();
                return;
            }

        }

        private void ProcessShootRoll(DiceRoll role)
        {
            gameState.TriggerPulls++;
            if (gameState.ChambersLoaded.Contains(role.RollResult))
            {
                OnPlayerShot(role.RollResult);
            }
            else
            {
                chatOutput.WritePlayerSurvives(gameState.CurrentPlayer);
            }

            if (gameState.TriggerPulls >= gameHost.Players.ActivePlayers.Count())
            {
                AddBullet(false);
            }

            SetNextPlayer();
        }

        private void SetNextPlayer()
        {
            if (gameHost.Players.ActivePlayers.Count(p => p.GetData().Alive) <= 1) return;

            if (gameState.ChambersLoaded.Any())
            {
                gameState.CurrentPlayer = gameHost.Players.GetNext(gameState.CurrentPlayer, p => p.GetData().Alive);
            }
            else
            {
                gameState.TriggerPulls = 0;
                if (config.GarleanRouletteRestartIfGunEmpties)
                {
                    Plugin.Log.Info("Skipping to first player: " + gameState.CurrentPlayer.FullName);
                    gameState.CurrentPlayer = GetNextRoundFirstPlayer();
                }
                else
                {
                    gameState.CurrentPlayer = gameHost.Players.GetNext(gameState.CurrentPlayer, p => p.GetData().Alive);
                }

            }

            Plugin.Log.Verbose("Setting next player: " + gameState.CurrentPlayer.FullName);
            SetupCurrentPlayerRoll();            
        }
        public void SetBet(long bet)
        {
            gameHost.ChatOutput.WriteChat($"Bet set to {gameState.Bet}");
            gameState.Bet = bet;
        }

        private void AddBullet(bool isFirstTime)
        {
            if (!isFirstTime && !gameState.DidSomeoneDieThisRound)
            {
                gameHost.ChatOutput.WriteChat("Everybody has survived so far... Let's up the stakes", minSpacingBeforeInMs: 2000);
            }
            gameState.DidSomeoneDieThisRound = false;
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
                    gameHost.ChatOutput.WriteChat($"The host spins the cylinder.");
                    bulletInserted = true;
                }
            }
        }

        private void ShufflePlayersBasedOnRolledOrder()
        {
            var ordered = gameHost.Players.Reorder(p => p.GetData().OrderRolled);
            Plugin.Log.Verbose($"New player order: {gameHost.Players.ActivePlayers.Select(p => p.FullName.GetFirstName()).Humanize()}");
        }

        public void OnWin()
        {
            MGPlayer? survivor = gameHost.Players.ActivePlayers.FirstOrDefault(p => p.GetData().Alive);
            if (survivor == null)
            {
                Plugin.Log.Warning("No survivors at the end of game. This should not happen.");
                return;
            }

            chatOutput.WriteWinner(survivor);
            gameState.Stage = GRStage.Winner;

            foreach (var player in gameHost.Players.ActivePlayers.Where(p => p != survivor))
            {
                bank.TransferInUse(player, survivor);
            }

            Plugin.Log.Warning($"{survivor.FullName} wins {survivor.Bank.InUse.Formatted()} gil! <se.15>");
            gameState.ChambersLoaded.Clear();
            chatOutput.WriteClearCylinder();
        }

        private void AddTestPlayers(GameHost host)
        {
#if DEBUG
            host.Players.AddPlayer("Pistachio Herald@Omega");
            host.Players.AddPlayer("Macalania Nut@Louisoix");
            host.Players.AddPlayer("Lion Around@Omega");
            bank.SetAllStored(host.Players, 69420000);
#endif
        }

        public void Dispose()
        {
            rollTracker.Dispose();
        }
    }
}
