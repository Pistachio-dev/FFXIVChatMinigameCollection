using Model.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace Model.Banking
{
    public class GilTransaction
    {        
        [Required]
        public PlayerOOGData HostPlayer { get; set; }

        public uint PlayerCashRecordId { get; set; }

        [Required]
        public PlayerCashRecord PlayerCashRecord { get; set; }

        [Required]
        public bool IsRealGil { get; set; }

        [Required]
        public bool IsHouseCut { get; set; }

        [Required]
        public long Amount { get; } // Positive amounts means the banked amount for the player increases
        
        [Required]
        public DateTime WhenUtc {  get; }

        public static GilTransaction NewFakeGilTransaction(PlayerOOGData host, PlayerOOGData patron, bool isHouseCut, long amount)
        {
            var newTransaction = new GilTransaction(host, patron, isHouseCut, amount);
            newTransaction.IsRealGil = false;

            return newTransaction;
        }

        public static GilTransaction NewRealGilTransaction(PlayerOOGData host, PlayerOOGData patron, bool isHouseCut, long amount)
        {
            var newTransaction = new GilTransaction(host, patron, isHouseCut, amount);
            newTransaction.IsRealGil = true;

            return newTransaction;
        }

        public GilTransaction(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, bool isHouseCut, long amount)
        {
            HostPlayer = hostPlayer;
            PlayerCashRecord = patronPlayer.CashRecord;
            IsHouseCut = isHouseCut;
            Amount = amount;
            WhenUtc = DateTime.UtcNow;
        }

        internal GilTransaction()
        {

        }
    }
}
