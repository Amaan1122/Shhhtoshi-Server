namespace ShhhToshiApp.DTOs.TaskDTOs
{
    public class ClaimHistoryDto
    {
        public Guid Id { get; set; }
        public int PointsClaimed { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ClaimedAt { get; set; }
    }

}
