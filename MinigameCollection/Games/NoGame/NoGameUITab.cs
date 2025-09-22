using DalamudBasics.Logging;
using Dalamud.Bindings.ImGui;
using MinigameCollection.Common.UICommon;
using System.Numerics;
using MinigameCollection.Common.GameBase;

namespace MinigameCollection.Games.NoGame
{
    public class NoGameUITab : GameUITabBase
    {
        private readonly GameBase gameBoard;
        private readonly NoGameAction actions;
        protected readonly Vector4 defaultColor = new Vector4(0.1f, 0.1f, 0.1f, 1);

        public NoGameUITab(ILogService logService, NoGameBoard gameBoard, NoGameAction actions) : base(logService, gameBoard, actions)
        {
            this.gameBoard = gameBoard;
            this.actions = actions;
        }

        public override void Draw()
        {
            DrawDefault();
        }
    }
}
