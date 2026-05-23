using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsPlayerData : GameSpecificPlayerData
    {
        public int Score { get; set; } = 0;
        public int OrderRolled { get; set; } = -1;
    }
}
