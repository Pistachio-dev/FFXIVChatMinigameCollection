
using Model.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection
{
    public class PlayerSet
    {
        private List<MGPlayer> _players;

        public List<MGPlayer> Players => _players;

        public PlayerSet()
        {
            _players = new();
        }

        public PlayerSet(List<MGPlayer> players)
        {
            _players = players; 
        }

        public bool AddPlayer(string fullName)
        {
            if (_players.Any(p => p.Fullname == fullName))
            {
                return false;
            }

            _players.Add(new MGPlayer(fullName));
            return true;
        }

        public MGPlayer? GetPlayer(string fullName)
        {
            var existing = _players.FirstOrDefault(p => p.Fullname == fullName);
            if (existing != null)
            {
                return existing;
            }

            return null;
        }

    }
}
