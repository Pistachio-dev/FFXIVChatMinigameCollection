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

        public GarleanRoulette(RollTracker rollTracker, Configuration config)
        {
            this.rollTracker = rollTracker;
            this.config = config;        }

        public override void DrawUI()
        {

            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.Gun, "Take next shot manually"))
            {
                grActions.CastRoll(gameState.CurrentPlayer!.FullName);
            }

            if (ImGuiComponents.IconButtonWithText(Dalamud.Interface.FontAwesomeIcon.ArrowUp, "Roll order"))
            {
                grActions.StartOrderRound();
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
            gameState = new GRGameState();
            gameState.Stage = GRStage.NotStarted;
            grActions = new GRActions(Host, gameState, rollTracker, config);
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
                        Plugin.Log.Info("Ending roll order phase");
                        ShufflePlayersBasedOnRolledOrder();
                        Plugin.Log.Info("Starting shooting phase");
                        gameState.Stage = GRStage.Shooting;
                    }
                        break;
                case GRStage.Shooting:
                    if (RemainingSurvivors() == 1)
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

        private int RemainingSurvivors()
        {
            return Host.Players.Players.Count(p => p.GetData().Alive);
        }

        private void ShufflePlayersBasedOnRolledOrder()
        {
            var ordered = Host.Players.Players.OrderBy(p => p.GetData().OrderRolled).ToList();
            Host.Players.Players.Clear();
            Host.Players.Players.AddRange(ordered);
            Plugin.Log.Verbose($"New player order: {Host.Players.Players.Select(p => p.FullName.GetFirstName()).Humanize()}");
        }
    }
}
