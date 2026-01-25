using Common.Banking.Interface;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Common
{
    public abstract class GameBase
    {
        public readonly ISessionPlayerManager playerManager;
        public readonly IGilBanksContainer banks;

        protected GameBase(ISessionPlayerManager playerManager, IGilBanksContainer banks)
        {
            this.playerManager = playerManager;
            this.banks = banks;
        }

        public abstract void Draw();

        public virtual void RegisterChatCommands(IChatGui chatGui) { }

        public virtual void RegisterRollCommands() 
        { 
        }
        
        
    }
}
