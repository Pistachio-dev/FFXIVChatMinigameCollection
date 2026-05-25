using DalamudBasics.Chat.Output;
using DalamudBasics.Configuration;
using DalamudBasics.Extensions;
using Model.Base;
using System.Collections.Generic;
using System.Linq;

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

        public void RequestThrow(MGPlayer player, int dartNumber)
        {
            chatOutput.WriteChat($"----------<se.3>");
            var message = $"{player.FullName.GetFirstName()}, time to /throw ({dartNumber} out of {config.DartsAmountPerTurn})";
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

        public void WritePlayerScore(MGPlayer player)
        {
            chatOutput.WriteChat($"Total score: {player.GetData().Score}");
        }

        public void WriteScoreTable(List<MGPlayer> players)
        {
            chatOutput.WriteChat("Score table: ");
            foreach (var player in players.OrderByDescending(p => p.GetData().Score))
            {
                chatOutput.WriteChat($"{player.GetData().Score.ToString("000")} points <=={player.FullName.GetFirstName()}:");
            }
        }
    }
}
