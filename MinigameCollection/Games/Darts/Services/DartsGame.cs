using Dalamud.Game.ClientState.Objects.SubKinds;
using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using MinigameCollection.Bank;
using MinigameCollection.Emotes;
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
        private readonly EmoteReaderHooks emoteReader;

        public DartsGame(IConfigurationService<Configuration> config, DartsGameState gameState, DartsActions actions, DartsUI ui, BankActions bank, EmoteReaderHooks emoteReader) {
            this.config = config.GetConfiguration();
            this.gameState = gameState;
            this.actions = actions;
            this.ui = ui;
            this.bank = bank;
            this.emoteReader = emoteReader;
        }

        public override void DrawUI()
        {
            ui.DrawUI();
        }

        public override void Initialize(GameHost host)
        {
            host.DiceManager.OnDiceRoll += DiceRollCallback;
            emoteReader.OnEmote += EmoteCallback;
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            Host.DiceManager.OnDiceRoll -= DiceRollCallback;
            emoteReader.OnEmote -= EmoteCallback;
            base.Dispose();
        }
        public void DiceRollCallback(DiceRoll roll)
        {
            Plugin.Log.Info($"Roll detected: {roll.PlayerFullName} rolled a {roll.RollResult} ({roll.Type})");
            actions.ProcessRoll(roll);
        }

        public void EmoteCallback(IPlayerCharacter instigator, ushort emoteId)
        {
            // Emote ids for throwing snowball is 86 (with target) and 87 (without target).
            Plugin.Log.Info("Emote detected: {Instigator} performed emote {EmoteId}", instigator.Name.TextValue, emoteId);
        }
    }
}
