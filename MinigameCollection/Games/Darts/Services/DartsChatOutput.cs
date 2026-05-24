using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
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
        private Configuration config;

        public DartsChatOutput(IChatOutput chatOutput, IConfigurationService<Configuration> configService)
        {
            this.chatOutput = chatOutput;
            config = configService.GetConfiguration();
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

            chatOutput.WriteChat($"Hit: {result.LandedNumber}{result.ActualMultiplier} for {result.GetPoints()} points!<se.7>");
        }

        public void PrintScore(MGPlayer player)
        {
            var message = $"{player.FullName.GetFirstName()}'s score: {player.GetData().Score}/{config.DartsTargetScore}";
            chatOutput.WriteChat(message);
        }

        public void ThrowNotOnSnow()
        {
            var message = $"Stand in the snow before you /throw";
            chatOutput.WriteChat(message);
        }

        public void WriteRollingPlayerOrder()
        {
            chatOutput.WriteChat("Rolling player order");
        }

        public void WritePlayerOrder(List<string> names)
        {
            chatOutput.WriteChat($"Order: {names.GetWordsSeparatedByArrows()}", null, 1000);
        }

        public void WriteBounce(int prevScore, int totalScore)
        {
            var message = $"Score: {totalScore}, over {config.DartsTargetScore}! Bounced back to {prevScore}.";
            chatOutput.WriteChat(message);
        }

        public void WriteWin(int position, string playerName)
        {
            string place = position switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => position + "th"
            };

            chatOutput.WriteChat($"Score: {config.DartsTargetScore}. {playerName} gets {place} place.");
        }

        public void WriteDartHit(MGPlayer player, DartResult result)
        {
            chatOutput.WriteChat($"{player.FullName.GetFirstName()} throws: {result}! Total score: {player.GetData().Score}");
        }
    }
}
