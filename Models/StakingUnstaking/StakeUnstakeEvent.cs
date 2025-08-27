using System;

namespace ShhhToshiApp.Models.StakingUnstaking
{
    public class StakeUnstakeEvent
    {
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public string Type { get; set; } // "Stake" or "Unstake"
        public decimal Amount { get; set; }
        public DateTime EventDate { get; set; }
        public string Status { get; set; } // "completed" or "pending"
    }
}
