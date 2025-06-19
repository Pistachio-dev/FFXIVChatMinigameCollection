using MinigameCollection.Common.GameBoardCommon;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Games.NoGame
{
    internal class NoGamePlayerInSession : PlayerInSession
    {
        public NoGamePlayerInSession(PlayerOOGData player) : base(player)
        {
        }

        public string AdditionalProperty1 { get; set; } = "property from subclass";
    }
}
