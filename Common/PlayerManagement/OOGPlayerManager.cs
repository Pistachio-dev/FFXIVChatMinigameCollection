using CommonServices.PlayerManagement.Interface;
using DalamudBasics.Extensions;
using DalamudBasics.Logging;
using Model.PlayerManagement;
using PersistentModel.Repository.Interface;

namespace CommonServices.PlayerManagement
{
    public class OOGPlayerManager : IOOGPlayerManager
    {
        private readonly IPlayerRepository playerRepo;
        private readonly ILogService log;

        public OOGPlayerManager(IPlayerRepository playerRepo, ILogService log)
        {
            this.playerRepo = playerRepo;
            this.log = log;
        }

        public PlayerOOGData? CreatePlayer(string name, string world)
        {
            var player = new PlayerOOGData(name, world);
            if (!playerRepo.CreatePlayer(player))
            {
                log.Error("Could not create new player and it does not exist. Can't add player to game");
                return null;
            }

            log.Info($"Player {player.FullName} created.");
            return playerRepo.GetPlayerWithCashRecord(player.FullName);

        }

        public PlayerOOGData? GetPlayerWithCashRecord(string fullName)
        {
            return playerRepo.GetPlayerWithCashRecord(fullName);
        }

        public bool UpdateIdentity(string fullName, string newName, string newWorld)
        {
            return playerRepo.UpdateAlias(fullName.GetNameOnly(), fullName.GetWorld(), new PlayerIdentifier(newName, newWorld));
        }
    }
}
