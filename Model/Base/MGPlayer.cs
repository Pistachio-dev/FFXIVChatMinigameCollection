using Model.Base.Bank;

namespace Model.Base
{
    public class MGPlayer
    {
        public MGPlayer(string fullName)
        {
            FullName = fullName;
        }

        private Dictionary<GameId, GameSpecificPlayerData> gameData = new();
        public string FullName { get; set; } = "Unset name";

        public bool Afk { get; set; } = false;

        public PlayerBankAccount Bank { get; set; } = new();

        public T GetData<T>(GameId id) where T : GameSpecificPlayerData, new()
        {
            if (gameData.ContainsKey(id))
            {
                T data = (T)gameData[id];
                return data;
            }

            InitData<T>(id);
            return (T)gameData[id];
        }

        public void InitData<T>(GameId id) where T : GameSpecificPlayerData, new()
        {
            SetData<T>(id, new T());
        }

        public void SetData<T>(GameId id, T updatedData) where T : GameSpecificPlayerData, new()
        {
            gameData[id] = updatedData;
        }
    }
}
