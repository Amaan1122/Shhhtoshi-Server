using ShhhToshiApp.Models.StakingUnstaking;

namespace ShhhToshiApp.Models.TaskSystem
{
    public class PointClaim
    {
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public decimal PointsClaimed { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ClaimedAt { get; set; }

        public WalletUser WalletUser { get; set; }

    }
}
