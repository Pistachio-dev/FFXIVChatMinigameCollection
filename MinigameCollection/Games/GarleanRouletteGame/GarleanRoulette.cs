using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using MinigameCollection.Bank;
using MinigameCollection.Dice;
using Model.Base;
using System.Linq;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GarleanRoulette : Game
    {
        public static readonly GameId Id = new GameId("Garlean Roulette");
        private readonly RollTracker rollTracker;
        private readonly Configuration config;
        private GRGameState gameState;
        private GRActions grActions;
        private readonly GRUI grui;
        private readonly BankActions bank;

        public GarleanRoulette(RollTracker rollTracker, IConfigurationService<Configuration> config, GRGameState gameState, GRActions grActions, GRUI grui, BankActions bank)
        {
            this.rollTracker = rollTracker;
            this.gameState = gameState;
            this.grActions = grActions;
            this.grui = grui;
            this.bank = bank;
            this.config = config.GetConfiguration();
        }

        public override void DrawUI()
        {
            grui.DrawUI();
        }

        public override void Initialize(GameHost host)
        {
            host.DiceManager.OnDiceRoll += PlayerTriggerPull;
            var firstPlayer = host.Players?.GetFirst();
            if (firstPlayer != null)
            {
                gameState.CurrentPlayer = firstPlayer;
            }

            //AddTestPlayers(host);
            gameState.Stage = GRStage.NotStarted;
            Plugin.Log.Info($"{nameof(GarleanRoulette)} initialized.");
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
            switch (gameState.Stage)
            {
                case GRStage.NotStarted:
                    return;

                case GRStage.RollingOrder:
                    if (HaveAllRolledOrder())
                    {
                        grActions.FinishOrderAndStartShooting();
                    }
                    break;

                default:
                    break;
            }
        }

        private void PlayerTriggerPull(DiceRoll roll)
        {
            Plugin.Log.Info("Player trigger pull");
            grActions.ProcessRoll(roll);
        }

        private bool HaveAllRolledOrder()
        {
            return !Host.Players.Players.Any(p => p.GetData().OrderRolled == -1);
        }
    }
}
