using Model.Base;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MinigameCollection
{
    public class PlayerSet
    {
        [JsonInclude]
        private List<MGPlayer> _players { get; set; } = new();

        [JsonIgnore]
        public List<MGPlayer> Players => _players;

        public PlayerSet()
        {
            _players = new();
        }

        public PlayerSet(List<MGPlayer> players)
        {
            _players = players;
        }

        public void Restore(PlayerSet storedCopy)
        {
            _players.Clear();
            _players.AddRange(storedCopy.Players);
        }

        public PlayerSet Reorder<T>(Func<MGPlayer, T> comparer)
        {
            var reordered = _players.OrderBy(comparer).ToList();
            _players.Clear();
            _players.AddRange(reordered);

            return this;
        }

        public MGPlayer? GetFirst()
        {
            return _players.FirstOrDefault();
        }

        public MGPlayer GetNext(MGPlayer? current, Func<MGPlayer, bool>? conditionNeeded = null)
        {
            if (current == null)
            {
                throw new Exception("Trying to get next player, but current one is null");
            }

            bool loopedAround = false;
            int playersChecked = 0;
            if (conditionNeeded == null)
            {
                conditionNeeded = (player) => true;
            }

            var currentIndex = _players.IndexOf(current) + 1;

            while (playersChecked < _players.Count)
            {
                if (currentIndex == _players.Count && !loopedAround)
                {
                    currentIndex = 0;
                    loopedAround = true;
                }

                Plugin.Log.Warning($"Checking player {currentIndex}: {_players[currentIndex].FullName}");
                if (!_players[currentIndex].Afk && conditionNeeded(_players[currentIndex]))
                {
                    return _players[currentIndex];
                }

                currentIndex++;
                playersChecked += 1;
            }

            throw new Exception($"Could not get next player. Current player is {current.FullName}");

            Log.Warning($"Next player {current.FullName} is the previous one");
            return current;
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

        public List<MGPlayer> GetNonAfkPlayers()
        {
            return Players.Where(p => !p.Afk).ToList();
        }

        public void Remove(MGPlayer player)
        {
            _players.Remove(player);
        }
    }
}
