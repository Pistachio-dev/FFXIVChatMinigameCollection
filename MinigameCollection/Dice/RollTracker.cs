using DalamudBasics.Chat.Output;
using DalamudBasics.DiceRolling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Dice
{
    public class RollTracker
    {
        private bool acceptNextRollWithoutChecking = false;
        public RollTracker(IChatOutput chatOutput)
        {
            this.chatOutput = chatOutput;
        }
        public delegate void AwaitedRollCallback(DiceRoll roll);
        private record AwaitedRoll(string rollerFullName, AcceptedRollType type, int outOf, AwaitedRollCallback callback);

        private Queue<AwaitedRoll> awaitedRollQueue = new();
        private readonly IChatOutput chatOutput;

        public void QueueExpectedRoll(string rollerFullName, AcceptedRollType type, int outOf, AwaitedRollCallback callback)
        {
            var record = new AwaitedRoll(rollerFullName, type, outOf, callback);
            awaitedRollQueue.Enqueue(record);
            Plugin.Log.Info($"Queued awaited roll. {RollRecordToString}");
        }

        public void Reset()
        {
            awaitedRollQueue.Clear();
            acceptNextRollWithoutChecking = false;
        }

        // In case the house needs to roll for the player, forces the next roll to be accepted regardless of whom is it.
        public void AcceptNextRollRegardless()
        {
            acceptNextRollWithoutChecking = true;            

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

            if (acceptNextRollWithoutChecking || 
                (roll.PlayerFullName == expected.rollerFullName
                && roll.OutOf == expected.outOf
                && DiceRollTypeMatches(roll, expected)))
            {
                Plugin.Log.Info($"Roll accepted, running callback. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");
                awaitedRollQueue.Dequeue();
                acceptNextRollWithoutChecking = false;
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
