using Model.Base;

namespace MinigameCollection.Games.GarleanRouletteGame
{
    internal class GRPlayerData : GameSpecificPlayerData
    {
        public int OrderRolled { get; set; } = -1;

        public int Roll { get; set; }

        public bool Alive { get; set; } = true;

        public void Reset()
        {
            OrderRolled = -1;
            Roll = -1;
            Alive = true;
        }
    }
}
