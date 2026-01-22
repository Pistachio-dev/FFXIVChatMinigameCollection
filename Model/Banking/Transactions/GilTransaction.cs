using Model.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace Model.Banking.Transactions
{
    public class GilTransaction
    {        
        [Required]
        public PlayerOOGData HostPlayer { get; set; }

        [Required]
        public PlayerOOGData TargetPlayer { get; set; }

        [Required]
        public PlayerOOGData? SourcePlayer { get; set; }

        [Required]
        public bool IsRealGil { get; set; }

        [Required]
        public TransactionType Cause { get; set; }

        // Positive quantities = adding to the account
        [Required]
        public long InUseDiff { get; set; }

        [Required]
        public long StoredDiff { get; set; }

        [Required]
        public DateTime WhenUtc {  get; }

        public static GilTransaction FromCashIn(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, int amountAdded)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = true,
                InUseDiff = 0,
                StoredDiff = amountAdded,
                Cause = TransactionType.CashIn
            };
        }

        public static GilTransaction FromCashOut(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountRemoved)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = true,
                InUseDiff = 0,
                StoredDiff = amountRemoved * -1,
                Cause = TransactionType.CashOut
            };
        }

        public static GilTransaction FromManualSet(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountAdded, bool isRealGil)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = 0,
                StoredDiff = amountAdded * -1,
                Cause = TransactionType.ManuallySetStored
            };
        }

        public static GilTransaction FromIntoPlay(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountMoved, bool isRealGil)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = amountMoved,
                StoredDiff = amountMoved * -1,
                Cause = TransactionType.Play
            };
        }

        public static GilTransaction FromIntoBank(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountMoved, bool isRealGil)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = amountMoved * -1,
                StoredDiff = amountMoved,
                Cause = TransactionType.Bank
            };
        }

        public static GilTransaction FromChangeInGame(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountChanged, bool isRealGil)
        {
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = amountChanged,
                StoredDiff = 0,
                Cause = TransactionType.Bank,
            };
        }

        public static GilTransaction FromHouseCut(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountTakenInUse, long amountTakenStored, bool isRealGil)
        {
                return new GilTransaction
                {
                    HostPlayer = hostPlayer,
                    TargetPlayer = patronPlayer,
                    IsRealGil = isRealGil,
                    InUseDiff = amountTakenInUse,
                    StoredDiff = amountTakenStored,
                    Cause = TransactionType.Bank,
                };
            }

        internal GilTransaction()
        {
            WhenUtc = DateTime.UtcNow;
        }
    }
}
