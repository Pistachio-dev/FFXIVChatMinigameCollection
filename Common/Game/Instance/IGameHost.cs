using CommonServices.PlayerManagement.Interface;
using Model.PlayerManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServices.Game.Instance
{
    public interface IGameHost
    {
        public SessionPlayers Players { get; }     
    }
}
