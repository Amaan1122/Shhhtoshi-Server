namespace ShhhToshiApp.DTOs.TaskDTOs
{
    public class TaskCompleteHistoryDto
    {
        public Guid Id { get; set; }
        public string WalletAddress { get; set; }
        public Guid TaskId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
