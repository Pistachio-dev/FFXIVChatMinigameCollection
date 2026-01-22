using Common.Banking.Interface;
using CommonServices.Banking;
using CommonServices.Banking.Enum;
using CommonServices.Game.Instance;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Logging;
using Model.Banking;
using Model.Banking.Transactions;
using PersistentModel.Repository.Interface;
using System;


namespace Common.Banking
{
    internal class FakeGilBank : GilBank
    {
        private readonly ILogService log;
        private readonly IChatGui chatGui;
        private readonly ISessionPlayerManager playerManager;
        private readonly IPlayerRepository playerRepo;

        public FakeGilBank(ILogService logService,
                            IChatGui chatGui,
                            ISessionPlayerManager playerManager,
                            IPlayerRepository playerRepo): base(logService, chatGui, playerManager, playerRepo)
        {
            this.log = logService;
            this.chatGui = chatGui;
            this.playerManager = playerManager;
            this.playerRepo = playerRepo;
        }

        // Property accessor, no business logic
        protected override long GetInUseProperty(PlayerCashRecord record)
        {
            return record.InUseFake;
        }

        // Property accessor, no business logic
        protected override long GetStoredProperty (PlayerCashRecord record)
        {
            return record.StoredFake; 
        }

        // Property accessor, no business logic
        protected override void SetInUseProperty(PlayerCashRecord record, long value)
        {
            record.InUseFake = value;
        }

        // Property accessor, no business logic
        protected override void SetStoredProperty(PlayerCashRecord record, long value)
        {
            record.StoredFake = value;
        }

        protected override bool IsRealGil()
        {
            return false;
        }



        public override void StartBuyIn(string playerName, string playerWorld)
        {
            chatGui.PrintError($"No buy ins with fake cash");
            log.Info($"Attempted buy in in fake cash mode");
        }

        public override void StartCashOut(string playerName, string playerWorld)
        {
            chatGui.PrintError($"No cash outs with fake cash");
            log.Info($"Attempted cash out in fake cash mode");
        }       
    }
}
