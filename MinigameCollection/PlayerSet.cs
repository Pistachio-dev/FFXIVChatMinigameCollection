
using Model.Base;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinigameCollection
{
    public class PlayerSet
    {
        private readonly List<MGPlayer> _players = new();

        public List<MGPlayer> Players => _players;

        public PlayerSet()
        {
        }

        public PlayerSet(List<MGPlayer> players)
        {
            _players = players; 
        }

        public bool AddPlayer(string fullName)
        {
            Plugin.Log.Info("Attempting to add " + fullName);
            if (_players.Any(p => p.FullName == fullName))
            {
                Plugin.Log.Info(fullName + "is already added");
                return false;
            }

            _players.Add(new MGPlayer(fullName));
            Plugin.Log.Info(fullName + "is added");

            return true;
        }

        public MGPlayer? GetPlayer(string fullName)
        {
            var existing = _players.FirstOrDefault(p => p.FullName == fullName);
            if (existing != null)
            {
                return existing;
            }

            return null;
        }

    }
}
