using Dalamud.Plugin.Services;
using DalamudBasics.Configuration;
using DalamudBasics.DiceRolling;
using DalamudBasics.Extensions;
using ECommons.Configuration;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using MinigameCollection.Dice;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace MinigameCollection.Games.Common
{
    public class CommonActions
    {
        private readonly RollTracker rollTracker;
        private readonly IObjectTable objectTable;
        private readonly GameHost gameHost;
        private readonly Configuration config;


        public CommonActions(RollTracker rollTracker, IObjectTable objectTable, GameHost gameHost, IConfigurationService<Configuration> configService)
        {
            this.rollTracker = rollTracker;
            this.objectTable = objectTable;
            this.gameHost = gameHost;
            this.config = configService.GetConfiguration();
        }

        public void SetupRoll(AcceptedRollType acceptedTypes, int max, Action<DiceRoll> onRollDetected, MGPlayer? player = null)
        {
            bool isHouse = player == null;
            string playerFullName = isHouse ? "the house" : player!.FullName;
            Plugin.Log.Info("Roll expectation queued for " + playerFullName);
            rollTracker.QueueExpectedRoll(playerFullName, acceptedTypes, max, isHouse,
            (roll) => {
                if (!isHouse)
                {
                    gameHost.ChatOutput.WriteChat($"{(isHouse ? playerFullName : playerFullName.GetFirstName())} rolled {roll.RollResult}");
                }
                onRollDetected(roll);
            });
            gameHost.ChatOutput.WriteDiceCommand(100, config.DefaultOutputChatType == Dalamud.Game.Text.XivChatType.Alliance);

        }      
    }
}
