using Model.Banking;
using Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentModel.Repository.Interface
{
    internal class PlayerRepository : IPlayerRepository
    {
        public bool CreatePlayer(PlayerOOGData playerData)
        {
            throw new NotImplementedException();
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
