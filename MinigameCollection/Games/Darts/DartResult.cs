using DalamudBasics.DiceRolling;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts
{
    internal class DartResult
    {
        public DartResult(int landedNumber, int landedMultiplier)
        {
            LandedNumber = landedNumber;
            LandedMultiplier = landedMultiplier;
        }

        public int LandedNumber { get; set; }

        public int LandedMultiplier { get; set; }

        public int ActualMultiplier => GetMultiplier();

        public override string ToString()
        {
            if (LandedNumber == 21)
            {
                if (LandedMultiplier > 3)
                {
                    // Bullseye, dead on
                    return $"Bullseye, dead on! 50 points!!";
                }
                else
                {
                    // Bullseye ring
                    return $"Hit the bullseye ring! 25 points!";
                }
            }

            var multiplier = GetMultiplier();
            if (multiplier > 1)
            {
                return $"{LandedNumber}{multiplier}={GetPoints()}";
            }

            return $"{LandedNumber}";
        }
        public int GetPoints()
        {
            if (LandedNumber == 21)
            {
                if (LandedMultiplier > 3)
                {
                    // Bullseye, dead on
                    return 50;
                }
                else { 
                    // Bullseye ring
                    return 25;
                }
            }

            var multiplier = LandedMultiplier switch
            {
                0 => 0,
                > 3 and < 6 => 2,
                6 => 3,
                _ => 1,
                };

            return LandedNumber * multiplier;
        }

        public int GetMultiplier() 
        {
            if (LandedMultiplier == 6)
            {
                return LandedNumber == 21 ? 2 : 3;
            }
            if (LandedMultiplier > 3 && LandedMultiplier < 6) return 2;
            if (LandedMultiplier == 0)
            {
                Plugin.Log.Warning($"Landed an invalid multiplier: {LandedMultiplier}.");
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
