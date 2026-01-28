using Dalamud.Plugin.Services;
using MinigameCollection.Games;
using MinigameCollection.Games.MicroGameGame;
using MinigameCollection.Games.NoGameGame;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinigameCollection
{
    public class GameHost
    {
        public (GameId id, Func<IGame> builder)[] AvailableGames()
        {
            return [
                (NoGame.Id, () => new NoGame()),
                (Microgame.Id, () => new Microgame())
            ];
        }

        private IGame? activeGame;

        private PlayerSet players;

        public PlayerSet Players => players;

        public GameHost(PlayerSet players, IFramework framework)
        {
            this.players = players;
        }

        public bool HasGame()
        {
            return activeGame != null;
        }

        public void StartGame(GameId gameId)
        {
            (GameId id, Func<IGame> constructor) = AvailableGames().FirstOrDefault(p => p.id.Equals(gameId));
            activeGame = constructor();
            activeGame.SafeInitialize(this);
        }

        public void Update()
        {
            activeGame?.SafeUpdate();
        }

        public void DrawUI()
        {
            activeGame?.SafeDrawUI();
        }
    }
}
