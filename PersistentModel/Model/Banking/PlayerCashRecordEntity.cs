using PersistentModel.Model.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace PersistentModel.Model.Banking
{
    public class PlayerCashRecordEntity
    {
        [Key]
        public uint Id { get; set; }

        public uint PlayerOOGDataID { get; set; }

        public PlayerOOGDataEntity PlayerOOGData { get; set; }

        [Required]
        public long StoredReal { get; set; }

        [Required]
        public long StoredFake { get; set; }

        [Required]
        public long InUseReal {  get; set; }

        [Required]
        public long InUseFake { get; set; }

        [Required]
        public List<GilTransactionEntity> History { get; set; } = new();

        public PlayerCashRecordEntity(PlayerOOGDataEntity playerOOgData)
        {
            PlayerOOGData = playerOOgData;
            PlayerOOGDataID = playerOOgData.Id;
        }
    }
}
