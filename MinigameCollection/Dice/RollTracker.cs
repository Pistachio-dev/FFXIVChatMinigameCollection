using Dalamud.Plugin.Services;
using DalamudBasics.Chat.Output;
using DalamudBasics.DiceRolling;
using System.Collections.Generic;
using DalamudBasics.Extensions;
using System;

namespace MinigameCollection.Dice
{
    public class RollTracker : IDisposable
    {
        private bool acceptNextRollWithoutCheckingPlayer = false;

        public RollTracker(DiceRollManager diceManager, IChatOutput chatOutput, IObjectTable objectTable)
        {
            this.diceManager = diceManager;
            this.chatOutput = chatOutput;
            this.objectTable = objectTable;
        }

        public delegate void AwaitedRollCallback(DiceRoll roll);
        private record AwaitedRoll(string rollerFullName, AcceptedRollType type, int outOf, bool rolledByHouse, AwaitedRollCallback callback);

        private Queue<AwaitedRoll> awaitedRollQueue = new();
        private readonly DiceRollManager diceManager;
        private readonly IChatOutput chatOutput;
        private readonly IObjectTable objectTable;

        public void Hook()
        {
            diceManager.OnDiceRoll += ProcessRoll;
        }

        public void Dispose()
        {
            diceManager.OnDiceRoll -= ProcessRoll;
        }

        public void ClearQueue()
        {
            awaitedRollQueue.Clear();
        }

        /// <summary>
        /// Stores information of a roll expected on the hook soon
        /// </summary>
        /// <param name="rollerFullName"></param>
        /// <param name="type">/dice or /random</param>
        /// <param name="outOf">Max inclusive value</param>
        /// <param name="rollAsHouse">The roll will be done by the plugin hosting player</param>
        /// <param name="callback">What to do once the roll is received</param>
        public void QueueExpectedRoll(string rollerFullName, AcceptedRollType type, int outOf, bool rollAsHouse, AwaitedRollCallback callback)
        {
            var record = new AwaitedRoll(rollerFullName, type, outOf, rollAsHouse, callback);
            awaitedRollQueue.Enqueue(record);
            Plugin.Log.Info($"Queued awaited roll. {RollRecordToString(record)}");
        }

        public void Reset()
        {
            awaitedRollQueue.Clear();
            acceptNextRollWithoutCheckingPlayer = false;
        }

        public void ProcessRoll(DiceRoll roll)
        {
            if (awaitedRollQueue.Count == 0)
            {
                return;
            }

            Plugin.Log.Info($"Processing roll. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");

            AwaitedRoll expected = awaitedRollQueue.Peek();
            Plugin.Log.Info($"Checking match with: {RollRecordToString(expected)}");

            if (roll.OutOf == expected.outOf
                && DiceRollTypeMatches(roll, expected)
                && (roll.PlayerFullName == expected.rollerFullName
                || (expected.rolledByHouse && roll.PlayerFullName == objectTable.LocalPlayer?.GetFullName()))
                || acceptNextRollWithoutCheckingPlayer)
            {
                Plugin.Log.Info($"Roll accepted, running callback. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");
                awaitedRollQueue.Dequeue();
                acceptNextRollWithoutCheckingPlayer = false;
                expected.callback(roll);
                return;
            }

            Plugin.Log.Info($"Roll does not match any expected. From: {roll.PlayerFullName}, Type:{roll.Type} {roll.RollResult} out  of: {roll.OutOf}");
            Plugin.Log.Info($"Expected: from: {expected.rollerFullName}, Type:{expected.type}  out  of: {expected.outOf}");
        }
        
        // In case the house needs to roll for the player, forces the next roll to be accepted regardless of whom is it.
        public void AcceptNextRollRegardless()
        {
            acceptNextRollWithoutCheckingPlayer = true;
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
