using Model.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace Model.Banking
{
    public class PlayerCashRecord
    {
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

        public PlayerCashRecord(PlayerOOGData owner)
        {
            PlayerOOGData = owner;
            StoredReal = 0;
            StoredFake = 0;
            History = new();
        }

        public void AddTransaction(GilTransaction transaction)
        {
            History.Add(transaction);
        }

        internal PlayerCashRecord() { }
    }
}
