using CommonServices.PlayerManagement.Interface;
using MinigameCollection.Common.GameBoardCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.PlayerManagement
{
    public class SessionPlayers<T> where T: IGameSpecificPlayerData
    {
        public List<PlayerInSession<T>> InGame = new();
        public List<PlayerInSession<T>> Spectating = new();
        public PlayerInSession<T>? Dealer = null;

        public bool RemovePlayer(string fullName)
        {
            PlayerInSession<T>? player = InGame.FirstOrDefault(x => x.FullName == fullName);
            if (player == null)
            {
                player = Spectating.FirstOrDefault(x => x.FullName == fullName);
                if (player == null)
                {
                    return false;
                }
                Spectating.Remove(player);
                return true;
            }

            InGame.Remove(player);
            return true;
        }

        public bool IsPlayerInSession(string name, string world)
        {
            return InGame.Any(p => p.Is(name, world)) || Spectating.Any(p => p.Is(name, world));
        }

        public PlayerInSession<T>? GetPlayer(string fullName)
        {
            return InGame.Concat(Spectating).FirstOrDefault(p => p.FullName == fullName);
        }

        public bool MoveToSpectator(string fullName)
        {
            return ChangeCollection(fullName, InGame, Spectating);
        }

        public bool MoveToActivePlayer(string fullName)
        {
            return ChangeCollection(fullName, Spectating, InGame);
        }

        private bool ChangeCollection(string fullName, List<PlayerInSession<T>> origin, List<PlayerInSession<T>> target)
        {
            PlayerInSession<T>? player = origin.FirstOrDefault(p => p.FullName == fullName);
            if (player == null)
            {
                return false;
            }
            origin.Remove(player);
            target.Add(player);

            return true;
        }
    }
}
