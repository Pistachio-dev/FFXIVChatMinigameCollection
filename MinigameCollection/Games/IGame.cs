using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    public interface IGame
    {

        void SafeInitialize(GameHost gameHost);
        void SafeUpdate();
        void SafeDrawUI();
    }
}
