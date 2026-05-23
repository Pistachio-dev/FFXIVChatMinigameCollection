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
        public static string Description { get; } = "Players take turns to shoot themselves with a partially loaded revolver. Last man standing wins all.";
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
            grActions.ResetGame(host);
        }

        private void PlayerTriggerPull(DiceRoll roll)
        {
            Plugin.Log.Info("Player trigger pull");
            grActions.ProcessRoll(roll);
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
                    else
                    {
                        
                    }
                    break;

                default:
                    break;
            }
        }

        private bool HaveAllRolledOrder()
        {
            return !Host.Players.ActivePlayers.Any(p => p.GetData().OrderRolled == -1);
        }
    }
}
