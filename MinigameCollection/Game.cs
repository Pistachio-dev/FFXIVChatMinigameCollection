using MinigameCollection.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection
{
    public abstract class Game : IGame
    {
        private bool initialized = false;

        public virtual void SafeInitialize(PlayerSet players)
        {
            if (!initialized) { initialized = true; }
            Initialize(players);
        }

        public virtual void SafeUpdate()
        {
            if (!initialized)
            {
                throw new Exception($"Game instance is not initialized");
            }
            Update();
        }

        public virtual void SafeDrawUI() { }

        public abstract void DrawUI();
        public abstract void Initialize(PlayerSet players);
        public abstract void Update();
    }
}
