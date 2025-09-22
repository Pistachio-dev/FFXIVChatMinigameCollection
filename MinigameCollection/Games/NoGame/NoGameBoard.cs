using DalamudBasics.Configuration;
using MinigameCollection.Common.GameBase;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Games.NoGame
{
    public class NoGameBoard : GameBase
    {
        public NoGameBoard(IConfigurationService<Configuration> configurationService, 
           PlayersInSessionManager playerManager) : base(configurationService, playerManager)
        {
            CustomProperty = "Custom initialized property";
        }

        public string CustomProperty {  get; set; }
    }
}
