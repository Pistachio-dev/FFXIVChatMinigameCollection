using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    public interface IGame: IDisposable
    {

        void SafeInitialize(GameHost gameHost);
        void SafeUpdate();
        void SafeDrawUI();
    }
}
