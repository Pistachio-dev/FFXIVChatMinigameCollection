using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    public interface IGame
    {
        public GameId Id { get; }

        void Initialize();
        void Update();
        void DrawUI();
    }
}
