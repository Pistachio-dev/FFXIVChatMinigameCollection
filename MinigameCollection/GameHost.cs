using MinigameCollection.Games;
using Model.Base;
using Model.Microgame;
using System;
using System.Collections.Generic;

namespace MinigameCollection
{
    public class GameHost
    {
        public static (GameId id, Func<IGame> builder)[] AvailableGames =
        [
            (new GameId("No Game"), () => new NoGame()),
            (new GameId("Microgame"), () => new Microgame())
        ];

        private IGame? activeGame;

        private PlayerSet players;

        public PlayerSet Players => players;
        public GameHost()
        {
            this.players = new PlayerSet();
        }

        public bool HasGame()
        {
            return activeGame != null;
        }

        public void StartGame(IGame game)
        {
            activeGame = game;
            activeGame.Initialize(players);
        }

        public void Update()
        {
            activeGame?.Update();
        }

        public void DrawUI()
        {
            activeGame?.DrawUI();
        }
    }
}
