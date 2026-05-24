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

        public int TotalTurnScore { get; set; } = 0;

        public void Reset()
        {
            ResetRound();
        }

        public void ResetRound()
        {
            DartsThrownThisTurn = 0;
            TotalTurnScore = 0;

        }
    }
}
