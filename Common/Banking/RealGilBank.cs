using Common.Banking.Interface;
using CommonServices.Banking;
using CommonServices.Banking.Enum;
using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Logging;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Layer;
using Model.Banking;
using PersistentModel.Repository.Interface;
using System;

namespace Common.Banking
{
    internal class RealGilBank : GilBank
    {
        private IPlayerRepository playerRepo;

        public RealGilBank(ILogService logService,
                            IChatGui chatGui,
                            ISessionPlayerManager playerManager,
                            IPlayerRepository playerRepo): base(logService, chatGui, playerManager, playerRepo)
        {
            playerRepo = playerRepo;
        }

        public override void StartBuyIn(string playerName, string playerWorld)
        {
            throw new NotImplementedException();
        }

        public override void StartCashOut(string playerName, string playerWorld)
        {
            throw new NotImplementedException();
        }

        protected override long GetInUseProperty(PlayerCashRecord record)
        {
            throw new NotImplementedException();
        }

        protected override long GetStoredProperty(PlayerCashRecord record)
        {
            throw new NotImplementedException();
        }

        protected override bool IsRealGil()
        {
            throw new NotImplementedException();
        }

        protected override void SetInUseProperty(PlayerCashRecord record, long value)
        {
            throw new NotImplementedException();
        }

        protected override void SetStoredProperty(PlayerCashRecord record, long value)
        {
            throw new NotImplementedException();
        }
    }
}
