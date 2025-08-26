namespace ShhhToshiApp.DTOs
{
    public class WalletInfoDto
    {
        public string WalletAddress { get; set; }
        public decimal TONBalance { get; set; }
        public decimal StakedAmount { get; set; }
        public DateTime LastStakedAt { get; set; }
        public DateTime LastUnstakedAt { get; set; }
        public DateTime JoinedAt { get; set; }
        public int Points { get; set; }
    }

}
