using DalamudBasics.Chat.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.Slots
{
    public class SlotsResultProcessing
    {
        public SlotsResultProcessing(IChatOutput chatOutput)
        {
            this.chatOutput = chatOutput;
        }

        public record PayoutSettings(string symbol, float payoutFor1, float payoutFor2, float payoutFor3);

        public Dictionary<SlotsSymbols, PayoutSettings> payoutSettings = new()
        { 
            {SlotsSymbols.PlaystationX, new PayoutSettings("", 0, 0, 0) },
            {SlotsSymbols.PlaystationSquare, new PayoutSettings("",0, 2, 20) },
            {SlotsSymbols.PlaystationTriangle, new PayoutSettings("",0, 3, 30) },
            {SlotsSymbols.PlaystationCircle, new PayoutSettings("",0, 4, 60) },
            
            {SlotsSymbols.Sprout, new PayoutSettings("",2, 10, 100) },
            {SlotsSymbols.HQ, new PayoutSettings("", 0, 2, 500) },
            {SlotsSymbols.Lucky7, new PayoutSettings("", 5, 250, 2500) },
            {SlotsSymbols.Ilvlsync, new PayoutSettings("", 0, 0, 2) },
        };
        private readonly IChatOutput chatOutput;

        public void PrintPayoutTable()
        {
            foreach (var kvp in payoutSettings)
            {
                StringBuilder sb = new StringBuilder();
                if (kvp.Value.payoutFor1 > 0)
                {
                    sb.Append(kvp.Value.symbol);
                    sb.Append("x");
                    sb.Append(kvp.Value.payoutFor1);
                    sb.Append("  ");
                }
                if (kvp.Value.payoutFor2 > 1)
                {
                    sb.Append(kvp.Value.symbol);
                    sb.Append(kvp.Value.symbol);
                    sb.Append("x");
                    sb.Append(kvp.Value.payoutFor2);
                    sb.Append("  ");
                }
                if (kvp.Value.payoutFor3 > 1)
                {
                    sb.Append(kvp.Value.symbol);
                    sb.Append(kvp.Value.symbol);
                    sb.Append(kvp.Value.symbol);
                    sb.Append("x");
                    sb.Append(kvp.Value.payoutFor3);
                }
                if (sb.Length > 0)
                {
                    chatOutput.WriteChat(sb.ToString());
                }
            }


        }
    }
}
