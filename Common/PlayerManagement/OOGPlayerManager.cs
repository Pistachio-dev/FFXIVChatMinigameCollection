using CommonServices.PlayerManagement.Interface;
using DalamudBasics.Extensions;
using Model.PlayerManagement;
using PersistentModel.Repository.Interface;

namespace CommonServices.PlayerManagement
{
    public class OOGPlayerManager : IOOGPlayerManager
    {
        private readonly IPlayerRepository playerRepo;

        public OOGPlayerManager(IPlayerRepository playerRepo)
        {
            this.playerRepo = playerRepo;
        }
        public PlayerOOGData? GetPlayer(string fullName)
        {
            return playerRepo.GetPlayerWithCashRecord(fullName);
        }

        public bool UpdateIdentity(string fullName, string newName, string newWorld)
        {
            return playerRepo.UpdateAlias(fullName.GetNameOnly(), fullName.GetWorld(), new PlayerIdentifier(newName, newWorld));
        }
    }
}
