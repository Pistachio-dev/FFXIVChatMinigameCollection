using Dalamud.Game.Text;
using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Listener;
using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
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
    public class SlotsGameActions : IDisposable
    {
        private readonly GameHost host;
        private readonly IChatOutput chatOutput;
        private readonly SlotsGameState gameState;
        private readonly BankActions bank;
        private readonly RollTracker rollTracker;
        private readonly IChatListener chatListener;
        private readonly IChatGui chatGui;
        private readonly IConfigurationService<Configuration> config;
        private readonly SlotsResultProcessing slotsResultProcessing;
        private readonly Regex BetRegex = new Regex("^bet ([0-9\\.,]+)([km]?)$");

        public SlotsGameActions(GameHost host, IChatOutput chatOutput, SlotsGameState gameState, BankActions bank,
            RollTracker rollTracker, IChatListener chatListener, IChatGui chatGui, IConfigurationService<Configuration> config,
            SlotsResultProcessing slotsResultProcessing)
        {
            this.host = host;
            this.chatOutput = chatOutput;
            this.gameState = gameState;
            this.bank = bank;
            this.rollTracker = rollTracker;
            this.chatListener = chatListener;
            this.chatGui = chatGui;
            this.config = config;
            this.slotsResultProcessing = slotsResultProcessing;
            this.chatListener = chatListener;
        }


        public void AddTriggers()
        {
            chatListener.AddPreprocessedMessageListener(OnMessageDelegate);
            rollTracker.Hook();
        }

        public void PrintPayoutTable()
        {
            slotsResultProcessing.PrintPayoutTable();
        }

        private void Bet(MGPlayer player, long amount)
        {
            if (gameState.Stage != SlotsGameStage.Idle)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, the slot machine is busy right now.{amount.Formatted()}");
                return;
            }
            if (player.Bank.Stored < amount)
            {
                chatOutput.WriteChat($"{player.FullName.GetFirstName()}, you don't have enough funds stored to bet {amount.Formatted()}");
                return;
            }

            gameState.Stage = SlotsGameStage.Rolling;
            chatOutput.WriteChat($"{player.FullName.GetFirstName()} bets {amount.Formatted()} on slots!");
            gameState.Bet = amount;
            gameState.Player = player;
            bank.Draw(player, amount);

            SetupRolls();
            TriggerSlotsRolls();
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

        public void TriggerSlotsRolls()
        {
            for (int i = 0; i < 3; i++)
            {
                host.ChatOutput.WriteDiceCommand(999, config.GetConfiguration().DefaultOutputChatType == XivChatType.Alliance);
            }
        }

        private void OnRoll(DiceRoll result)
        {
            Plugin.Log.Info("OnRoll");
            gameState.Results[gameState.ResultCount] = result.RollResult;
            gameState.ResultCount += 1;

            if (gameState.ResultCount == 3)
            {
                gameState.ResultCount = 0;
                gameState.Stage = SlotsGameStage.ShowingResult;
                OutputAndBankResult();
            }

        }

        private void OutputAndBankResult()
        {
            slotsResultProcessing.ProcessPayout(gameState.Results);
            gameState.Stage = SlotsGameStage.ShowingResult;
            bank.StoreAll(gameState.Player ?? throw new Exception("Trying to store money of a null player"));
            chatOutput.WriteChat($"Your bank: {gameState.Player.Bank.Stored.Formatted()} gil. <se.15>");
            gameState.Reset();
            gameState.Stage = SlotsGameStage.Idle;
        }

        private void OnMessageDelegate(XivChatType type, string sender, string message, DateTime receivedAt)
        {
            var match = BetRegex.Match(message.ToLower());
            if (!match.Success)
            {
                return;
            }

            gameState.Bet = GetBetAmount(match) * GetMultiplier(match);
            Plugin.Log.Info($"Bet detected from {sender}: {gameState.Bet}");            

            var player = host.Players.GetPlayer(sender);
            if (player == null)
            {
                Plugin.Log.Warning($"Bet detected, but {sender} is not in the game.");
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
            rollTracker.Dispose();
        }
    }
}
