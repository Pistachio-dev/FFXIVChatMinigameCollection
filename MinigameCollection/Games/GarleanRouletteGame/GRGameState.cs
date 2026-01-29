using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRGameState
    {
        public MGPlayer? CurrentPlayer { get; set; }

        public List<int> ChambersLoaded { get; set; } = new();

        public GRStage Stage = GRStage.RollingOrder;

        public bool AwaitingOwnRoll { get; set; }
    }
}
