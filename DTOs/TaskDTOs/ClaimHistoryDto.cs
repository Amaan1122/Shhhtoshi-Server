namespace ShhhToshiApp.DTOs.TaskDTOs
{
    public class ClaimHistoryDto
    {
        public Guid Id { get; set; }
        public decimal PointsClaimed { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ClaimedAt { get; set; }
    }

}
