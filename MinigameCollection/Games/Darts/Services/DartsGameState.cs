using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsGameState
    {
        public long Bet { get; set; } = 0;

        public MGPlayer? CurrentPlayer { get; set; }

        public int DartsThrownThisTurn { get; set; } = 0;

        public DartResult? LastDartHit { get; set; } = null;
        
        public DartsStage Stage { get; set; } = DartsStage.RollingOrder;

        public void Reset()
        {
            ResetRound();
        }

        public void ResetRound()
        {
            Plugin.Log.Warning("Resetto!");
            DartsThrownThisTurn = 0;
            LastDartHit = null;

        }
    }
}
