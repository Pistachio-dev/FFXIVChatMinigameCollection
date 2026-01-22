using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Banking.Transactions
{
    public enum CurrencyType
    {
        InGame, // e.g. bets in blackjack
        InGameReserve, //e.g. buyouts in poker
        InBank // Nontaxable. There for convenience
    }
}
