using Common.Banking.Interface;
using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Bindings.ImGui;
using MinigameCollection.Common;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinigameCollection.Games.PureRNG
{
    public class PureRNGGame : GameBase
    {
        private PlayerInSession currentPlayer;

        public PureRNGGame(ISessionPlayerManager playerManager, IGilBanksContainer banks) : base(playerManager, banks)
        {
            
        }

        public override void Draw()
        {
            ImGui.TextUnformatted("asdf");
        }
    }
}
