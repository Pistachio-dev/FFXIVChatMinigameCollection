using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Base
{
    public class MGPlayer
    {
        public MGPlayer(string fullName)
        {
            Fullname = fullName;
        }

        private Dictionary<GameId, GameSpecificPlayerData> gameData = new();
        public string Fullname { get; set; } = "Unset name";

        public GameSpecificPlayerData GetData(GameId id)
        {
            return gameData[id]; 
        }

        public void SetData(GameId id, GameSpecificPlayerData updatedData)
        {
            gameData[id] = updatedData;
        }
    }
}
