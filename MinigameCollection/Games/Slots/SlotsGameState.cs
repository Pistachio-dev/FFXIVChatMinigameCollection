using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Slots
{
    public class SlotsGameState
    {
        public MGPlayer? Player { get; set; }
        public long Bet { get; set;  }

        public int[] Results { get; set; } = new int[3];

        public int ResultCount = 0;

        public SlotGameStage Stage { get; set; }
    }
}
