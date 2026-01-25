using CommonServices.PlayerManagement.Interface;
using Dalamud.Plugin.Services;
using DalamudBasics.Extensions;
using DalamudBasics.Logging;
using ECommons.GameHelpers;
using Model.Banking;
using Model.Banking.Transactions;
using Model.PlayerManagement;
using PersistentModel.Model;
using PersistentModel.Model.Banking;
using PersistentModel.Model.PlayerManagement;
using PersistentModel.Repository.Interface;

namespace CommonServices.PlayerManagement
{
    public class OOGPlayerManager : IOOGPlayerManager
    {
        private readonly IPlayerRepository playerRepo;
        private readonly ILogService log;
        private readonly IObjectTable objectTable;

        public OOGPlayerManager(IPlayerRepository playerRepo, ILogService log, IObjectTable objectTable)
        {
            this.playerRepo = playerRepo;
            this.log = log;
            this.objectTable = objectTable;
        }

        public PlayerOOGData? GetOrCreateHostPlayer()
        {
            var host = objectTable.LocalPlayer;
            if (host == null) { return null; }

            var retrieved = GetPlayerWithCashRecord(host.GetFullName());
            if  (retrieved == null)
            {
                return CreatePlayer(host.GetNameWithWorld(), host.GetWorldName());
            }

            return retrieved;
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

        // This does NOT do any business logic. Call it with an already changed player
        public bool UpdateCashRecord(PlayerOOGData updatedPlayer, GilTransaction newTransaction)
        {
            return playerRepo.UpdateCashRecord(updatedPlayer, newTransaction);
        }
    }
}
