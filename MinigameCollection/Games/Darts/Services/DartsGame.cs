using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using MinigameCollection.Bank;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsGame : Game
    {
        public static readonly GameId Id = new GameId("Darts");
        public const string Description = "/throw to roll a dart hit. Points are awarded based on where the dart hits. " +
            "First to reach the target score wins. If you would go over the target score, your dart score is not added.";
        private readonly Configuration config;
        private readonly DartsGameState gameState;
        private readonly DartsActions actions;
        private readonly DartsUI ui;
        private readonly BankActions bank;

        public DartsGame(IConfigurationService<Configuration> config, DartsGameState gameState, DartsActions actions, DartsUI ui, BankActions bank) {
            this.config = config.GetConfiguration();
            this.gameState = gameState;
            this.actions = actions;
            this.ui = ui;
            this.bank = bank;
        }

        public override void DrawUI()
        {
            ui.DrawUI();
        }

        public override void Initialize(GameHost host)
        {
            host.DiceManager.OnDiceRoll += DiceRollCallback;
        }

        public override void Update()
        {
        }

        public void DiceRollCallback(DiceRoll roll)
        {
            Plugin.Log.Info($"Roll detected: {roll.PlayerFullName} rolled a {roll.RollResult} ({roll.Type})");
            actions.ProcessRoll(roll);
        }
    }
}
