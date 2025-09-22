using Dalamud.Plugin.Services;
using DalamudBasics.Chat.ClientOnlyDisplay;
using MinigameCollection.Common.GameActionsCommon;
using MinigameCollection.Common.GameBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinigameCollection.Games.NoGame
{
    public class NoGameAction : GameActionsBase
    {
        public NoGameAction(IClientChatGui chatGui, GameBase gameBoard) : base(gameBoard)
        {
            ChatGui = chatGui;
        }

        public IClientChatGui ChatGui { get; }

        public bool ThisIsAGameActionForThisOne()
        {
            ChatGui.PrintError("Action executed");
            return true;
        }
    }
}
