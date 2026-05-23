using DalamudBasics.DiceRolling;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts
{
    internal class DartResult
    {
        public int LandedNumber { get; set; }

        public int LandedMultiplier { get; set; }

        public int Multiplier() 
        {
            if (LandedMultiplier == 6) return 3;
            if (LandedMultiplier > 3 && LandedMultiplier < 6) return 2;
            if (LandedMultiplier == 0)
            {
                Plugin.Log.Warning("Landed an invalid multiplier: {LandedMultiplier}.");
                return 0;
            }
            return 1;
        }

        public void SetLandedNumber(DiceRoll roll)
        {
            if (roll.OutOf != 21)
            {
                Plugin.Log.Warning("Landing roll received, but it was not out of 21, it was out of " + roll.OutOf);
                return;
            }

            LandedNumber = roll.RollResult;            
        }

        public void SetLandedMultiplier(DiceRoll roll)
        {
            if (roll.OutOf != 3)
            {
                Plugin.Log.Warning("Multiplier roll received, but it was not out of 3, it was out of " + roll.OutOf);
                return;
            }

            LandedMultiplier = roll.RollResult;
        }
    }
}
