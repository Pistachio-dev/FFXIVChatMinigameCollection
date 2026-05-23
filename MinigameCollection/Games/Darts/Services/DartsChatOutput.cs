using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using InteropGenerator.Runtime;
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

        public void ThrowDetected(DartResult result)
        {
            if (result.LandedNumber == 21)
            {
                if (result.LandedMultiplier > 3)
                {
                    chatOutput.WriteChat($"Bullseye, dead on! 50 points!!<se.15>");
                    return;

                }

                chatOutput.WriteChat($"Hit the bullseye ring! 25 points!<se.7>!");
                return;
            }

            chatOutput.WriteChat($"Hit: {result.LandedNumber} x{result.ActualMultiplier} for {result.GetPoints()} points!<se.7>");
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
