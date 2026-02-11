using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.ClientOnlyDisplay;
using DalamudBasics.Chat.Listener;
using DalamudBasics.Chat.Output;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using Model.Base;
using System;
using System.Text.RegularExpressions;

namespace MinigameCollection.Games.Slots
{
    internal class SlotsGameActions : IDisposable
    {
        private readonly GameHost host;
        private readonly IChatOutput chatOutput;
        private readonly SlotsGameState gameState;
        private readonly BankActions bank;
        private readonly RollTracker rollTracker;
        private readonly IChatListener chatListener;
        private readonly IChatGui chatGui;
        private readonly Regex BetRegex = new Regex("bet ([0-9\\.,]+)([km]?)");

        public SlotsGameActions(GameHost host, IChatOutput chatOutput, SlotsGameState gameState, BankActions bank, RollTracker rollTracker, IChatListener chatListener, IChatGui chatGui)
        {
            this.host = host;
            this.chatOutput = chatOutput;
            this.gameState = gameState;
            this.bank = bank;
            this.rollTracker = rollTracker;
            this.chatListener = chatListener;
            this.chatGui = chatGui;
            this.chatListener = chatListener;
        }


        public void AddChatTrigger()
        {
            chatListener.AddPreprocessedMessageListener(OnMessageDelegate);
        }

        private void Bet(MGPlayer player, long amount)
        {
            if (gameState.Stage != SlotsGameStage.Idle)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, the slot machine is buy right now.{amount.Formatted()}");
            }
            if (player.Bank.Stored < amount)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, you don't have enough funds stored to bet {amount.Formatted()}");
                return;
            }

            gameState.Bet = amount;
            gameState.Player = player;
            bank.Draw(player, amount);

            SetupRolls();
        }

        public void Reset()
        {
            gameState.Reset();
            rollTracker.ClearQueue();
        }

        private void SetupRolls()
        {
            for (int i = 0; i < 3; i++)
            {
                rollTracker.QueueExpectedRoll(host.GetHostPlayerFullName(), AcceptedRollType.Dice, 999, true, OnRoll);
            }
        }

        private void OnRoll(DiceRoll result)
        {
            gameState.Results[gameState.ResultCount] = result.RollResult;
            gameState.ResultCount += 1;

            if (gameState.ResultCount == 3)
            {
                gameState.Stage = SlotsGameStage.ShowingResult;
                OutputAndBankResult();
            }
            
        }

        private void OutputAndBankResult()
        {
            Plugin.Log.Info($"Results: {gameState.Results.Humanize()}");
            gameState.Stage = SlotsGameStage.Idle;
        }

        private void OnMessageDelegate(XivChatType type, string sender, string message, DateTime receivedAt)
        {
            var match = BetRegex.Match(message);
            if (match.Success)
            {
                gameState.Bet = GetBetAmount(match) * GetMultiplier(match);
                Plugin.Log.Info($"Bet detected from {sender}: {gameState.Bet}");
            }

            var player = host.Players.GetPlayer(sender);
            if (player == null)
            {
                chatGui.Print($"Bet detected, but {sender} is not in the game.");
                return;
            }

            gameState.Player = player;
            Bet(player, gameState.Bet);
        }

        private int GetBetAmount(Match patternMatch)
        {
            var matchedText = patternMatch.Groups[1].Captures[0].Value;
            matchedText = matchedText.Replace(".", string.Empty).Replace(",", string.Empty);

            return int.Parse(matchedText);
        }

        private int GetMultiplier(Match patternMatch)
        {
            var multiplierMatch = patternMatch.Groups[2].Captures[0];
            if (multiplierMatch.Value == "k")
            {
                return 1000;
            }
            if (multiplierMatch.Value == "m")
            {
                return 1000 * 1000;
            }

            return 1;
        }

        public void Dispose()
        {
            chatListener.Dispose();
        }
    }
}
