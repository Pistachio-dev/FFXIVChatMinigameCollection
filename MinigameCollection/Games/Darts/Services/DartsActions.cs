using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using MinigameCollection.Emotes;
using MinigameCollection.Games.Common;
using MinigameCollection.Save;
using Model.Base;
using System;
using System.Linq;

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
        private readonly EmoteExpectedQueue expectedEmoteQueue;
        private readonly Configuration config;
        private const int OrderRollMax = 100;
        // /throw without snow: 85
        // /throw with snow, targeted: 86
        // /throw with snow, untargeted: 87
        private readonly int[] acceptedThrowEmoteIds = [85, 86, 87];


        public DartsActions(GameHost gameHost, DartsGameState gameState, RollTracker rollTracker,
            IConfigurationService<Configuration> config, DartsChatOutput chatOutput, BankActions bank, SaveManager saveManager, CommonActions commonActions,
            EmoteExpectedQueue emoteQueue)
        {
            this.gameHost = gameHost;
            this.gameState = gameState;
            this.rollTracker = rollTracker;
            this.config = config.GetConfiguration();
            this.chatOutput = chatOutput;
            this.bank = bank;
            this.saveManager = saveManager;
            this.commonActions = commonActions;
            this.expectedEmoteQueue = emoteQueue;
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
                commonActions.SetupRoll(AcceptedRollType.Any, OrderRollMax, (roll) => SetPlayerOrderRoll(player, roll.RollResult), true, null);
            }
        }

        public void ProcessRoll(DiceRoll roll)
        {
            rollTracker.ProcessRoll(roll);
        }

        public void ProcessEmote(string instigatorName, int emoteId)
        {
            if (!acceptedThrowEmoteIds.Contains(emoteId))
            {
                Plugin.Log.Verbose($"Emote {emoteId} is not a throw emote, ignoring");
                return;
            }

            if (instigatorName != gameState.CurrentPlayer?.FullName)
            {
                Plugin.Log.Verbose($"Emote instigator {instigatorName} is not the current player {gameState.CurrentPlayer}, ignoring");
                return;
            }

            ProcessThrow();
        }

        // Dart landing chain: 1
        private void ProcessThrow()
        {
            RollDartLanding();
        }
        // Dart landing chain: 2
        private void RollDartLanding()
        {
            // roll landing (1-20, 21 is bullseye)            
            commonActions.SetupRoll(AcceptedRollType.Any, 21, (diceRoll) => RollDartMultiplier(diceRoll.RollResult), true, gameState.CurrentPlayer);
        }

        // Dart landing chain: 3
        private void RollDartMultiplier(int landingResult)
        {
            // roll multiplier (1-3 is 1, 4-5 is 2, 6 is 3)
            commonActions.SetupRoll(AcceptedRollType.Any, 6, (diceRoll) => RunHitResult(landingResult, diceRoll.RollResult), true, gameState.CurrentPlayer);
        }

        // Dart landing chain: 4
        private void RunHitResult(int landingResult, int multiplierResult)
        {
            var hit = new DartResult(landingResult, multiplierResult);
            chatOutput.ThrowDetected(hit);
            gameState.DartsThrownThisTurn++;
            gameState.TotalTurnScore += hit.GetPoints();
            UpdateScoreAndWinners();
            NextTurn();
        }

        private void NextTurn()
        {
            var amountOfWinners = UpdateScoreAndWinners();
            if (IsEndOfGame(amountOfWinners))
            {
                EndGame();
                return;
            }

            // Next dart
            if (gameState.DartsThrownThisTurn == 3 || gameState.CurrentPlayer?.GetData().Place > 0)
            {
                // Next player
                gameState.ResetRound();
                NextPlayer();
            }

            SetUpThrowForCurrentPlayer();
        }

        private int UpdateScoreAndWinners()
        {
            var cpData = gameState.CurrentPlayer?.GetData();

            if (cpData == null)
            {
                Plugin.Log.Error("Current player has no player data!");
                return 0;
            }

            int scoreBeforeThrow = cpData.Score;
            int turnFullScore = cpData.Score + gameState.TotalTurnScore;
            int amountOfWinners = gameHost.Players.ActivePlayers.Count(p => p.GetData().Place > 0);


            if (turnFullScore > config.DartsTargetScore && config.DartsNeedExactThrow )
            {
                // Bounce
                chatOutput.WriteBounce(scoreBeforeThrow, turnFullScore);
                return amountOfWinners;
            }

            if (turnFullScore >= config.DartsTargetScore && !config.DartsNeedExactThrow)
            {
                // Win
                chatOutput.WriteWin(amountOfWinners, gameState.CurrentPlayer?.FullName ?? "Null player");
                cpData.Place = amountOfWinners + 1;
            }            

            cpData.Score = turnFullScore;
            gameState.CurrentPlayer?.SetData(cpData);

            return amountOfWinners;
        }

        private bool IsEndOfGame(int amountOfWinners)
        {
            // Either the only player wins, or n-1 player won
            int players = gameHost.Players.ActivePlayers.Count;
            if (players == 1 && amountOfWinners == 1) return true;
            if ((amountOfWinners == players - 1)) return true;
            return false;
        }
        private void EndGame()
        {
            foreach (var player in gameHost.Players.ActivePlayers)
            {
                var data = player.GetData();
                data.Reset();
                player.SetData(data);                
            }

            gameState.Reset();
        }

        private void NextPlayer()
        {
            gameHost.Players.GetNext(gameState.CurrentPlayer, p => p.GetData().Score != config.DartsTargetScore);
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
            gameState.CurrentPlayer = gameHost.Players.ActivePlayers.FirstOrDefault();
            if (gameState.CurrentPlayer == null)
            {
                Plugin.Log.Error("Attempting to start game, but no current player found");
                return;
            }
            SetUpThrowForCurrentPlayer();
        }

        private void SetUpThrowForCurrentPlayer()
        {
            gameState.ResetRound();
            chatOutput.RequestThrow(gameState.CurrentPlayer ?? throw new Exception ("Starting turn for null current player"));
            expectedEmoteQueue.ExpectEmote(gameState.CurrentPlayer?.FullName ?? "Null player", acceptedThrowEmoteIds, ProcessEmote);
        }
    }
}
