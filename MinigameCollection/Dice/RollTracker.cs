using DalamudBasics.DiceRolling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Dice
{
    public class RollTracker
    {
        public delegate void AwaitedRollCallback(DiceRoll roll);
        private record AwaitedRoll(string rollerFullName, AcceptedRollType type, int outOf, AwaitedRollCallback callback);

        private Queue<AwaitedRoll> awaitedRollQueue = new();

        public void QueueExpectedRoll(string rollerFullName, AcceptedRollType type, int outOf, AwaitedRollCallback callback)
        {
            var record = new AwaitedRoll(rollerFullName, type, outOf, callback);
            awaitedRollQueue.Enqueue(record);
            Plugin.Log.Info($"Queued awaited roll. {RollRecordToString}");
        }

        public void ProcessRoll(DiceRoll roll)
        {
            if (awaitedRollQueue.Count == 0)
            {
                return;
            }

            Plugin.Log.Info($"Processing roll. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");

            var expected = awaitedRollQueue.Peek();
            Plugin.Log.Info($"Checking match with: {RollRecordToString(expected)}");

            if (roll.PlayerFullName == expected.rollerFullName
                && roll.OutOf == expected.outOf
                && DiceRollTypeMatches(roll, expected))
            {
                Plugin.Log.Info($"Roll accepted, running callback. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");
                awaitedRollQueue.Dequeue();
                expected.callback(roll);
                return;
            }

            Plugin.Log.Info($"Roll does not match any expected. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");

        }

        private string RollRecordToString(AwaitedRoll roll)
        {
            return $"From: {roll.rollerFullName}, Type:{roll.type} Out  of: {roll.outOf}";

        }

        private bool DiceRollTypeMatches(DiceRoll roll, AwaitedRoll expected)
        {
            return expected.type == AcceptedRollType.Any
                    || (expected.type == AcceptedRollType.Random && roll.Type == DiceRollType.Random)
                    || (expected.type == AcceptedRollType.Dice && roll.Type == DiceRollType.Dice);
        }
    }
}
