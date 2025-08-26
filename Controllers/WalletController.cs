using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shhhtoshi.Api.DB;
using ShhhToshiApp.DTOs;
using ShhhToshiApp.Models.StakingUnstaking;

namespace ShhhToshiApp.Controllers
{
    [ApiController]
    [Route("api/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public WalletController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectWallet([FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            if (string.IsNullOrWhiteSpace(walletAddress))
                return BadRequest("Wallet address is required");

            var existing = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (existing != null)
                return Ok(new { message = "Wallet already connected" });

            var newUser = new WalletUser
            {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                // Seeding initial balance
                TONBalance = 1000,
                StakedAmount = 0,
                LastStakedAt = DateTime.UtcNow,
                LastUnstakedAt = DateTime.UtcNow,
                JoinedAt = DateTime.UtcNow
            };

            _dbContext.WalletUsers.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Wallet connected", walletAddress });
        }

        [HttpGet("walletInfo")]
        public async Task<ActionResult<WalletInfoDto>> GetWalletInfo(string walletAddress)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return NotFound();

            var response = new WalletInfoDto
            {
                WalletAddress = user.WalletAddress,
                TONBalance = user.TONBalance,
                StakedAmount = user.StakedAmount,
                LastStakedAt = user.LastStakedAt,
                LastUnstakedAt = user.LastUnstakedAt,
                Points = user.Points
            };

            return Ok(response);
        }
    }
}
