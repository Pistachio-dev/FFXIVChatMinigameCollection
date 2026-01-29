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

        public bool AwaitingOwnRoll { get; set; }

        public bool WinCondition()
        {
            return RemainingSurvivors() == 1;
        }

        private int RemainingSurvivors()
        {
            return gameHost.Players.Players.Count(p => p.GetData().Alive);
        }
    }
}
