using System;

namespace MinigameCollection.Emotes
{
    internal class ExpectedEmote
    {
        public delegate void EmoteCallback(string instigatorName, int emoteId);

        public ExpectedEmote(string instigatorName, int[] emoteIds, EmoteCallback callback)
        {
            EmoteIds = emoteIds;
            InstigatorName = instigatorName;
            Callback = callback;
        }
        public int[] EmoteIds { get; set; }
        public string InstigatorName { get; set; }

        public EmoteCallback Callback;
    }
}
