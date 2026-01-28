using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games
{
    public interface IGame
    {

        void SafeInitialize(PlayerSet players);
        void SafeUpdate();
        void SafeDrawUI();
    }
}
