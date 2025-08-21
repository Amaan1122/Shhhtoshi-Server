using ShhhToshiApp.Models.TaskSystem;

namespace ShhhToshiApp.Models.StakingUnstaking
{
    public class WalletUser
    {
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public decimal StakedAmount { get; set; }
        public decimal TONBalance { get; set; }
        public DateTime LastStakedAt { get; set; }
        public DateTime JoinedAt { get; set; }

        // New field for points
        public int Points { get; set; }

        public ICollection<TaskCompletion> TaskCompletions { get; set; }
        public ICollection<PointClaim> PointClaims { get; set; }


    }
}
