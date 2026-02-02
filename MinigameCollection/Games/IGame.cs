using System;

namespace MinigameCollection.Games
{
    public interface IGame : IDisposable
    {
        void SafeInitialize(GameHost gameHost);

        void SafeUpdate();

        void SafeDrawUI();
    }
}
