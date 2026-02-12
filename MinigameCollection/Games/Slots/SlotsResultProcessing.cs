using DalamudBasics.Chat.Output;
using MinigameCollection.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection.Games.Slots
{
    public class SlotsResultProcessing
    {
        private readonly GameHost gameHost;
        private IChatOutput chatOutput;
        private BankActions bank;
        private readonly SlotsGameState gameState;

        public SlotsResultProcessing(GameHost gameHost, IChatOutput chatOutput, BankActions bank, SlotsGameState gameState)
        {
            this.gameHost = gameHost;
            this.chatOutput = chatOutput;
            this.bank = bank;
            this.gameState = gameState;
        }

        public SlotsSymbol GetSymbolFromDiceResult(int dice)
        {
            int totalSymbols = 80 + 30 + 25 + 20 + 15 + 8 + 4 + 1;
            int cappedValue = dice % totalSymbols;

            if (cappedValue <= 80) return SlotsSymbol.Ilvlsync;
            if (cappedValue <= 80 + 30) return SlotsSymbol.PlaystationX;
            if (cappedValue <= 80 + 30 + 25) return SlotsSymbol.PlaystationSquare;
            if (cappedValue <= 80 + 30 + 25 + 20) return SlotsSymbol.PlaystationTriangle;
            if (cappedValue <= 80 + 30 + 25 + 20 + 15) return SlotsSymbol.PlaystationCircle;
            if (cappedValue <= 80 + 30 + 25 + 20 + 15 + 8) return SlotsSymbol.Sprout;
            if (cappedValue <= 80 + 30 + 25 + 20 + 15 + 8 + 4) return SlotsSymbol.HQ;
            if (cappedValue <= 80 + 30 + 25 + 20 + 15 + 8 + 4 + 1) return SlotsSymbol.Lucky7;

            return SlotsSymbol.Ilvlsync;
        }

        public void ProcessPayout(int[] results)
        {
            var player = gameState.Player ?? throw new Exception("Can't process payout: player is null!");
            var resultSymbols = results.Select(x => GetSymbolFromDiceResult(x)).ToList();
            var resultSymbolsGrouped = resultSymbols.GroupBy(x => x).ToList();
            int total = 0;

            string reels = "";
            foreach (var symbol in resultSymbols)
            {
                reels += $"[ {PayoutSettingsTable[symbol].symbol} ]";
                chatOutput.WriteChat($"{reels}<se.14>", minSpacingBeforeInMs: 1000);
            }
            foreach (var group in resultSymbolsGrouped)
            {                
                var count = group.Count();
                
                var symbol = group.Key;
                var payout = GetPayout(symbol, count);
                if (payout > 0)
                {
                    int payoutGil = (int)Math.Floor(gameState.Bet * payout);
                    chatOutput.WriteChat($"{PayoutSettingsTable[symbol].symbol}x{count}  bet x {payout} = {payoutGil.Formatted()} <se.7>");
                    total += payoutGil;
                }
            }

            bank.SetInUse(player, total);

            if (total > 0)
            {
                chatOutput.WriteChat($"Total payout: {total.Formatted()} gil! <se.15>");
            }
            else
            {
                chatOutput.WriteChat($"No luck this time. Try again! <se.5>");
            }
        }

        private float GetPayout(SlotsSymbol symbol, int appearances)
        {
            Plugin.Log.Info($"{symbol}: {appearances}-{PayoutSettingsTable[symbol].payoutFor1}-{PayoutSettingsTable[symbol].payoutFor2}-{PayoutSettingsTable[symbol].payoutFor3}");
            switch (appearances)
            {
                case 1:
                    return PayoutSettingsTable[symbol].payoutFor1;
                case 2:
                    return PayoutSettingsTable[symbol].payoutFor2;
                case 3:
                    return PayoutSettingsTable[symbol].payoutFor3;
                default:
                    return 0;
            }
        }

        public record PayoutSettings(string symbol, float payoutFor1, float payoutFor2, float payoutFor3);

        public static Dictionary<SlotsSymbol, PayoutSettings> PayoutSettingsTable = new()
        { 
            {SlotsSymbol.PlaystationX, new PayoutSettings("", 0, 0, 0) },
            {SlotsSymbol.PlaystationSquare, new PayoutSettings("",0, 2, 20) },
            {SlotsSymbol.PlaystationTriangle, new PayoutSettings("",0, 3, 30) },
            {SlotsSymbol.PlaystationCircle, new PayoutSettings("",0, 4, 60) },
            
            {SlotsSymbol.Sprout, new PayoutSettings("",2, 10, 100) },
            {SlotsSymbol.HQ, new PayoutSettings("", 0, 2, 500) },
            {SlotsSymbol.Lucky7, new PayoutSettings("", 5, 250, 2500) },
            {SlotsSymbol.Ilvlsync, new PayoutSettings("", 0, 0, 1) },
        };

        public void PrintPayoutTable()
        {
            foreach (var kvp in PayoutSettingsTable)
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
