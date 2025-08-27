using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shhhtoshi.Api.DB;
using ShhhToshiApp.Services;

namespace ShhhToshiApp.Controllers
{
    [ApiController]
    [Route("api/")]
    public class StakeUnstakeController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly TaskCompletionService _taskCompletionService;

        public StakeUnstakeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _taskCompletionService = new TaskCompletionService(dbContext);
        }

        [HttpPost("stake")]
        public async Task<IActionResult> Stake([FromHeader(Name = "X-Wallet-Address")] string walletAddress, [FromBody] decimal stakeAmount)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return NotFound();
            if (user.TONBalance < stakeAmount) return BadRequest("Insufficient TON Balance");

            user.StakedAmount += stakeAmount;
            user.TONBalance -= stakeAmount;
            user.LastStakedAt = DateTime.UtcNow;

            // Record event
            _dbContext.StakeUnstakeEvents.Add(new Models.StakingUnstaking.StakeUnstakeEvent {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                Type = "Stake",
                Amount = stakeAmount,
                EventDate = DateTime.UtcNow,
                Status = "completed"
            });

            // check if this is first stake for the user
            await _taskCompletionService.CompleteTaskIfNotAlready(walletAddress, "First Stake");

            // check if user has staked 100+ TON overall (only once)
            if (user.StakedAmount >= 100)
            {
                await _taskCompletionService.CompleteTaskIfNotAlready(walletAddress, "Stake 100+ TON");
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { user.StakedAmount });
        }

        [HttpPost("unstake")]
        public async Task<IActionResult> Unstake([FromHeader(Name = "X-Wallet-Address")] string walletAddress, [FromBody] decimal unStakeAmount)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return BadRequest("Not Found");
            if (user.StakedAmount < unStakeAmount) return BadRequest("Insufficient staked balance");

            user.StakedAmount -= unStakeAmount;
            user.TONBalance += unStakeAmount;
            user.LastUnstakedAt = DateTime.UtcNow;

            // Record event
            _dbContext.StakeUnstakeEvents.Add(new Models.StakingUnstaking.StakeUnstakeEvent {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                Type = "Unstake",
                Amount = unStakeAmount,
                EventDate = DateTime.UtcNow,
                Status = "completed"
            });

            await _dbContext.SaveChangesAsync();
            return Ok(new { user.StakedAmount });
        }
        // GET: Return stake/unstake history for the wallet
        [HttpGet("stakeUnstake/history")]
        public async Task<IActionResult> GetStakeUnstakeHistory([
            FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return NotFound("Wallet not registered");

            var stakeEvents = await _dbContext.StakeUnstakeEvents
                .Where(e => e.WalletAddress == walletAddress)
                .OrderByDescending(e => e.EventDate)
                .Select(e => new
                {
                    e.Id,
                    e.Type, // "Stake" or "Unstake"
                    e.Amount,
                    e.EventDate,
                    e.Status // "completed" or "pending"
                })
                .ToListAsync();

            return Ok(stakeEvents);
        }
    }
}
