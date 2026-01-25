using CommonServices.PlayerManagement.Interface;
using Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Game.Instance
{
    public interface IGameInstance<T> where T: IGameSpecificPlayerData
    {
        public SessionPlayers<T> Players { get; }     
    }
}
