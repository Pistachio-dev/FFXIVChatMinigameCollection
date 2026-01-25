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

        public static GilTransaction FromManuallySettingStored(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long newAmount, bool isRealGil)
        {
            var existingAmount = isRealGil ? patronPlayer.CashRecord.StoredReal : patronPlayer.CashRecord.StoredFake;
            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = 0,
                StoredDiff = existingAmount,
                Cause = TransactionType.ManuallySetStored
            };
        }

        public static GilTransaction FromStoredToInUse(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountMoved, bool isRealGil)
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

        public static GilTransaction FromInUseToStored(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountMoved, bool isRealGil)
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


        public static GilTransaction FromManuallySettingInUse(PlayerOOGData hostPlayer, PlayerOOGData patronPlayer, long amountChanged, bool isRealGil)
        {
            var existingAmount = isRealGil ? patronPlayer.CashRecord.InUseReal : patronPlayer.CashRecord.InUseFake;

            return new GilTransaction
            {
                HostPlayer = hostPlayer,
                TargetPlayer = patronPlayer,
                IsRealGil = isRealGil,
                InUseDiff = existingAmount,
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
