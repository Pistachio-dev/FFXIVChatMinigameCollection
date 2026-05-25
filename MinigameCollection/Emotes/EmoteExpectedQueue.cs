using System.Collections.Generic;
using System.Linq;
using static MinigameCollection.Emotes.ExpectedEmote;

namespace MinigameCollection.Emotes
{
    internal class EmoteExpectedQueue
    {
        private Queue<ExpectedEmote> expectedEmotes = new();

        public void Reset()
        {
            expectedEmotes.Clear();
        }

        public void ExpectEmote(string instigatorName, int[] emoteIds, EmoteCallback callback)
        {
            var expectedEmote = new ExpectedEmote(instigatorName, emoteIds, callback);
            expectedEmotes.Enqueue(expectedEmote);
        }

        public void CheckEmote(string instigatorName, int emoteId)
        {
            if (expectedEmotes.Count == 0) { return; }
            var expectedEmote = expectedEmotes.Peek();
            if (expectedEmote.InstigatorName == instigatorName && expectedEmote.EmoteIds.Contains(emoteId))
            {
                Plugin.Log.Info($"Emote matched: Instigator={instigatorName}, EmoteId={emoteId}");
                expectedEmote.Callback(instigatorName, emoteId);
                expectedEmotes.Dequeue();

            }
        }
    }
}
