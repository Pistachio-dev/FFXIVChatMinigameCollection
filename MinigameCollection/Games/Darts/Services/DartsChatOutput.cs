using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Darts.Services
{
    internal class DartsChatOutput
    {
        private IChatOutput chatOutput;

        public DartsChatOutput(IChatOutput chatOutput)
        {
            this.chatOutput = chatOutput;
        }

        public void RequestThrow(MGPlayer player)
        {
            var message = $"{player.FullName.GetFirstName()}, time to /throw";
            chatOutput.WriteChat(message);
        }

        public void ThrowNotOnSnow()
        {
            var message = $"Stand in the snow before you /throw";
            chatOutput.WriteChat(message);
        }

        public void RollingPlayerOrder()
        {
            chatOutput.WriteChat("Rolling player order");
        }

        public void WritePlayerOrder(List<string> names)
        {
            chatOutput.WriteChat($"Order: {names.GetWordsSeparatedByArrows()}", null, 1000);
        }
    }
}
