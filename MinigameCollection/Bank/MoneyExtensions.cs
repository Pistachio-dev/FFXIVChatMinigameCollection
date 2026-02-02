using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Bank
{
    public static class MoneyExtensions
    {
        public static string Formatted(this int value)
        {
            return BigCashFormat((long)value);
        }

        public static string Formatted(this long value)
        {
            return BigCashFormat(value);
        }

        public static string Formatted(this decimal value)
        {
            return BigCashFormat(value);
        }

        private static string BigCashFormat(long amount)
        {
            return $"{amount:n0}";
        }

        public static string BigCashFormat(decimal amount)
        {
            return $"{amount:n0}";
        }
    }
}
