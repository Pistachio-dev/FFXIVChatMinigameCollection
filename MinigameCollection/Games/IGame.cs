using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    public interface IGame
    {
        public GameId Id { get; }

        void Initialize(PlayerSet players);
        void Update();
        void DrawUI();
    }
}
