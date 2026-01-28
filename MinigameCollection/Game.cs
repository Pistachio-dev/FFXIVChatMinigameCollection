using Dalamud.Bindings.ImGui;
using MinigameCollection.Games;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection
{
    public abstract class Game : IGame
    {
        private bool initialized = false;

        public void SafeInitialize(GameHost host)
        {
            if (!initialized) { initialized = true; }
            Initialize(host);
        }

        public void SafeUpdate()
        {
            if (!initialized)
            {
                throw new Exception($"Game instance is not initialized");
            }
            Update();
        }

        public void SafeDrawUI()
        {
            DrawUI();
        }

        public abstract void DrawUI();
        public abstract void Initialize(GameHost host);
        public abstract void Update();
    }
}
