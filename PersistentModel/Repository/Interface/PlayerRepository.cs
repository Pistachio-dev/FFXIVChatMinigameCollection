using Model.Banking;
using Model.PlayerManagement;
using PersistentModel.Model;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Repository.Interface
{
    internal class PlayerRepository : IPlayerRepository
    {
        private MinigameCollectionDbContext context;
        private EntityMapper mapper;

        public PlayerRepository(MinigameCollectionDbContext context)
        {
            this.context = context;
            this.mapper = new EntityMapper();
        }
        public bool CreatePlayer(PlayerOOGData playerData)
        {
            PlayerOOGDataEntity entity = EntityMapper.Mapper.Map<PlayerOOGDataEntity>(playerData);
            PlayerCashRecordEntity cashRecord = new PlayerCashRecordEntity(entity);
            entity.CashRecord = cashRecord;
            context.PlayerCashRecords.Add(cashRecord);
            context.PlayerOOGData.Add(entity);
            context.SaveChanges();
            
            return true;
        }

        public bool GetPlayerWithCashRecord(string playerFullName)
        {
            throw new NotImplementedException();
        }

        public bool RemovePlayer(string playerFullName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAlias(PlayerIdentifier newAlias)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCashRecord(PlayerOOGData player, PlayerCashRecord cashRecord, GilTransaction newTransaction)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePlayer(PlayerOOGData playerData)
        {
            throw new NotImplementedException();
        }
    }
}
