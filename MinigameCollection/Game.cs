using DalamudBasics.DiceRolling;
using MinigameCollection.Games;
using System;

namespace MinigameCollection
{
    public abstract class Game : IGame, IDisposable
    {
        private bool initialized = false;

        private GameHost? host;
        protected GameHost Host => initialized && host != null ? host : throw new Exception("Attemptint go access an uninitialized game host");

        public void SafeInitialize(GameHost host)
        {
            if (!initialized) { initialized = true; }
            this.host = host;
            Initialize(host);
            this.host.DiceManager.OnDiceRoll += OnDiceRoll;
            this.host.Framework.Update += (iframework) => SafeUpdate();
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

        public virtual void OnDiceRoll(DiceRoll roll)
        { }

        public void Dispose()
        {
            host.DiceManager.OnDiceRoll -= OnDiceRoll;
        }
    }
}
