using PersistentModel.Model.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace PersistentModel.Model.Banking
{
    public class PlayerCashRecord
    {
        [Key]
        public uint Id { get; set; }

        public uint PlayerOOGDataID { get; set; }

        public PlayerOOGData PlayerOOGData { get; set; }

        [Required]
        public long StoredReal { get; set; }

        [Required]
        public long StoredFake { get; set; }

        [Required]
        public long InUseReal {  get; set; }

        [Required]
        public long InUseFake { get; set; }

        [Required]
        public List<GilTransaction> History { get; set; } = new();

        public PlayerCashRecord()
        {
            StoredReal = 0;
            StoredFake = 0;
            History = new();
        }

        public void AddTransaction(GilTransaction transaction)
        {
            History.Add(transaction);
        }
    }
}
