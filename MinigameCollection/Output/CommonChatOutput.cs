using DalamudBasics.Chat.Output;
using DalamudBasics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentItemSearch;

namespace MinigameCollection.Output
{
    public class CommonChatOutput
    {
        private readonly IChatOutput chatOutput;
        private readonly PlayerSet players;

        public CommonChatOutput(IChatOutput chatOutput, PlayerSet players) {
            this.chatOutput = chatOutput;
            this.players = players;
        }

        public void WriteFinances()
        {
            var highestInUse = this.players.AllPlayers.Max(p => p.Bank.InUse);
            var highestStored = this.players.AllPlayers.Max(p => p.Bank.Stored);
            var inUseNumberFormat = CommonChatOutput.GetNumberFormat(highestInUse);
            var storedNumberFormat = CommonChatOutput.GetNumberFormat(highestStored);
            chatOutput.WriteChat($"==Money on the table:");
            foreach (var player in this.players.AllPlayers)
            {
                chatOutput.WriteChat($"In use: {player.Bank.InUse.ToString(inUseNumberFormat)} gil. Stored: {player.Bank.Stored.ToString(storedNumberFormat)} gil. <={player.FullName.GetFirstName()}");
            }
        }

        public static string GetNumberFormat(long highestNumber)
        {
            int highestLength = highestNumber.ToString().Length;

            StringBuilder s = new StringBuilder();

            int k = 3 - (highestLength % 3);
            for (int i = 0; i < highestLength;)
            {
                if (k >= 3)
                {
                    if (i != 0)
                    {
                        s.Append(",");
                    }

                    k = 0;
                }
                else
                {
                    s.Append("0");

                    k += 1;
                    i += 1;
                }
            }

            return s.ToString();
        }
    }
}
