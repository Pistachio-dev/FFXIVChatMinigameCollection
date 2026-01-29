using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal static class MGPlayerDataExtensions
    {
        public static GRPlayerData GetData(this MGPlayer player)
        {
            return player.GetData<GRPlayerData>(GarleanRoulette.Id);
        }

        public static void SetData(this MGPlayer player, GRPlayerData data)
        {
            player.SetData<GRPlayerData>(GarleanRoulette.Id, data);
        }
    }
}
