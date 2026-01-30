using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Components;
using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using Humanizer;
using MinigameCollection.Dice;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GarleanRoulette : Game
    {
        public static readonly GameId Id = new GameId("Garlean Roulette");
        private readonly RollTracker rollTracker;
        private readonly Configuration config;
        private GRGameState gameState;
        private GRActions grActions;

        public GarleanRoulette(RollTracker rollTracker, IConfigurationService<Configuration> config, GRGameState gameState, GRActions grActions)
        {
            this.rollTracker = rollTracker;
            this.gameState = gameState;
            this.grActions = grActions;
            this.config = config.GetConfiguration();
        }

        public override void DrawUI()
        {

            if (gameState.Stage == GRStage.Shooting)
            {
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Gun, "Take next shot manually"))
                {
                    grActions.SetupCurrentPlayerRoll(true);
                }
            }

            if (gameState.Stage == GRStage.NotStarted)
            {
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.ArrowUp, "Roll order"))
                {
                    grActions.StartOrderRound();
                }
            }
            if (gameState.Stage == GRStage.Winner)
            {
                ImGui.TextUnformatted(gameState.GetSurvivor().FullName);
                if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Repeat, "Go again")){
                    grActions.StartOrderRound();
                }
            }
        }

        public override void Initialize(GameHost host)
        {            
            host.DiceManager.OnDiceRoll += PlayerTriggerPull;            
            var firstPlayer = host.Players?.GetFirst();
            if (firstPlayer != null)
            {
                gameState.CurrentPlayer = firstPlayer;
            }

            AddTestPlayers(host);
            gameState.Stage = GRStage.NotStarted;
            Plugin.Log.Info($"{nameof(GarleanRoulette)} initialized.");
        }

        private void AddTestPlayers(GameHost host)
        {
            host.Players.AddPlayer("Pistachio Herald@Omega");
            host.Players.AddPlayer("Macalania Nut@Louisoix");
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
                case GRStage.Shooting:
                    if (gameState.WinCondition())
                    {
                        grActions.OnWin();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }

        private void PlayerTriggerPull(DiceRoll roll)
        {
            Plugin.Log.Warning("playerTriggerPull");
            grActions.ProcessRoll(roll);
        }

        private bool HaveAllRolledOrder()
        {
            return !Host.Players.Players.Any(p => p.GetData().OrderRolled == -1);
        }
    }
}
