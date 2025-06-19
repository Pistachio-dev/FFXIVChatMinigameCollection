using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common.GameActionsCommon
{
    public class GameActionsBase
    {
        private readonly GameBoardBase gameBoard;

        public GameActionsBase(GameBoardBase gameBoard)
        {
            this.gameBoard = gameBoard;
        }

        public void AddTargetPlayer()
        {
            gameBoard.PlayerManager.AddTargetPlayer();
        }

        public void RemovePlayer(string fullName)
        {
            gameBoard.PlayerManager.RemovePlayer(fullName);
        }
    }
}
