using Model.Base;

namespace MinigameCollection.Games.Darts.Services
{
    internal static class DartsPlayerDataExtensions
    {
        public static DartsPlayerData GetData(this MGPlayer player)
        {
            return player.GetData<DartsPlayerData>(DartsGame.Id);
        }

        public static void SetData(this MGPlayer player, DartsPlayerData data)
        {
            player.SetData<DartsPlayerData>(DartsGame.Id, data);
        }
    }
}
