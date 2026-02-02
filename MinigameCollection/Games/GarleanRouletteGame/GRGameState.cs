using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRGameState
    {
        public GRGameState(GameHost gameHost)
        {
            this.gameHost = gameHost;
        }
        public MGPlayer? CurrentPlayer { get; set; }

        public List<int> ChambersLoaded { get; set; } = new();

        public GRStage Stage = GRStage.RollingOrder;

        private readonly GameHost gameHost;

        public int TriggerPulls { get; set; } = 0;

        public bool DidSomeoneDieThisRound = false;

        public long Bet { get; set; } = 0;

        public bool WinCondition()
        {
            return RemainingSurvivors() == 1;
        }

        private int RemainingSurvivors()
        {
            return gameHost.Players.GetNonAfkPlayers().Count(p => p.GetData().Alive);
        }

        public MGPlayer GetSurvivor()
        {
            return gameHost.Players.GetNonAfkPlayers().First(p => p.GetData().Alive);
        }
    }
}
