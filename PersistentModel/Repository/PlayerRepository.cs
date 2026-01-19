using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Banking;
using Model.PlayerManagement;
using PersistentModel.Model;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Repository
{
    internal class PlayerRepository : IPlayerRepository
    {
        private MinigameCollectionDbContext context;

        public PlayerRepository(MinigameCollectionDbContext context)
        {
            this.context = context;
        }
        public bool CreatePlayer(PlayerOOGData playerData)
        {
            var existing = context.PlayerOOGData.FirstOrDefault(p => p.Name == playerData.Name && p.World == playerData.World);
            if (existing != null)
            {
                return false;
            }
            var entity = EntityMapper.Mapper.Map<PlayerOOGDataEntity>(playerData);
            var cashRecord = new PlayerCashRecordEntity();
            entity.CashRecord = cashRecord;
            context.PlayerCashRecords.Add(cashRecord);
            context.PlayerOOGData.Add(entity);
            context.SaveChanges();
            
            return true;
        }

        public PlayerOOGData? GetPlayerWithCashRecord(string playerFullName)
        {
            if (!playerFullName.TryGetSplitName(out string name, out string world))
            {
                return null;
            }
            var existing = context.PlayerOOGData.AsNoTracking().Include(p => p.CashRecord).FirstOrDefault(p => p.Name == name && p.World == world);
            if (existing == null)
            {
                return null;
            }

            PlayerOOGData dataMapped = EntityMapper.Mapper.Map<PlayerOOGData>(existing);

            return dataMapped;

        }        

        public bool UpdateAlias(string name, string world, PlayerIdentifier newAlias)
        {
            var playerEntity = context.PlayerOOGData.FirstOrDefault(p => p.Name == name && p.World == world);
            if (playerEntity == null)
            {
                return false;
            }

            playerEntity.Name = newAlias.Name;
            playerEntity.World = newAlias.World;
            var aliasEntity = EntityMapper.Mapper.Map<PlayerIdentifierEntity>(newAlias);
            playerEntity.PreviousIdentities.Add(aliasEntity);
            context.PlayerIdentifiers.Add(aliasEntity);
            context.SaveChanges();

            return true;
        }

        // This does NOT do any business logic. Call it with an already changed player
        public bool UpdateCashRecord(PlayerOOGData updatedPlayer, GilTransaction newTransaction)
        {
            var existing = context.PlayerOOGData.Include(p => p.CashRecord).ThenInclude(c => c.History).FirstOrDefault(p => p.Name == updatedPlayer.Name && p.World == updatedPlayer.World);
            if (existing == null)
            {
                return false;
            }


            var cashRecordEntity = context.PlayerCashRecords.Entry(existing.CashRecord);
            EntityMapper.Mapper.Map<PlayerCashRecord, PlayerCashRecordEntity>(updatedPlayer.CashRecord, existing.CashRecord);

            GilTransactionEntity gilTransactionEntity = EntityMapper.Mapper.Map<GilTransactionEntity>(newTransaction);
            
            context.GilTransactions.Add(gilTransactionEntity);
            context.SaveChanges();
            
            return true;
        }
    }
}
