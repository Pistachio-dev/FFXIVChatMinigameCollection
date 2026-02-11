using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using MinigameCollection.Games.Slots;
using Model.Base;
using System.Linq;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class SlotsGame : Game
    {
        public static readonly GameId Id = new GameId("Slots");
        public static string Description { get; } = "Slot machine. Type !bet <amount>, like !bet 50k, to roll. Payouts are hardcoded for now";
        private readonly RollTracker rollTracker;
        private readonly IConfigurationService<Configuration> config1;
        private readonly SlotsGameActions actions;
        private readonly SlotsGameState state;
        private readonly Configuration config;
        private readonly BankActions bank;
        private readonly SlotsGameUI ui;

        public SlotsGame(RollTracker rollTracker, IConfigurationService<Configuration> config, SlotsGameActions actions, SlotsGameState state, BankActions bank, SlotsGameUI ui)
        {
            this.rollTracker = rollTracker;
            config1 = config;
            this.actions = actions;
            this.state = state;
            this.bank = bank;
            this.ui = ui;
            this.config = config.GetConfiguration();
        }

        public override void DrawUI()
        {
            ui.DrawUI();
        }

        public override void Initialize(GameHost host)
        {
            state.Reset();
            actions.AddChatTrigger();
            AddTestPlayers(host);
            Plugin.Log.Info($"{nameof(SlotsGame)} initialized.");
        }

        private void AddTestPlayers(GameHost host)
        {
            host.Players.AddPlayer("Pistachio Herald@Omega");
            host.Players.AddPlayer("Macalania Nut@Louisoix");
            host.Players.AddPlayer("Lion Around@Omega");
            bank.SetAllStored(host.Players, 69420000);
        }

        public override void Update()
        {            
        }

        public override void Dispose()
        {
            actions.Dispose();
            base.Dispose();
        }
    }
}
