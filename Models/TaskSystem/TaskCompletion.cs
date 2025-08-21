using ShhhToshiApp.Models.StakingUnstaking;

namespace ShhhToshiApp.Models.TaskSystem
{
    public class TaskCompletion
    {
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public Guid TaskId { get; set; }
        public DateTime CompletedAt { get; set; }

        public TaskItem TaskItem { get; set; }
        public WalletUser WalletUser { get; set; }

    }
}
