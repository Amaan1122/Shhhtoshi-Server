using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shhhtoshi.Api.DB;
using ShhhToshiApp.Services;

namespace ShhhToshiApp.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class DailyCheckinController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly TaskCompletionService _taskCompletionService;

        public DailyCheckinController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _taskCompletionService = new TaskCompletionService(dbContext);
        }

        [HttpPost("daily-checkin")]
        public async Task<IActionResult> DailyCheckin([FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return NotFound();

            // Check if user has already checked in today
            var today = DateTime.UtcNow.Date;
            var dailyCheckinTask = await _dbContext.TaskItems.FirstOrDefaultAsync(t => t.Title == "Daily Check-in");
            if (dailyCheckinTask == null) return NotFound("Task not found");

            var alreadyCheckedIn = await _dbContext.TaskCompletions.AnyAsync(tc => tc.WalletAddress == walletAddress && tc.TaskId == dailyCheckinTask.Id && tc.CompletedAt.Date == today);
            if (alreadyCheckedIn)
            {
                return Ok(new { alreadyCheckedIn = true, message = "Already checked in today." });
            }

            // Mark as completed for today and award points
            _dbContext.TaskCompletions.Add(new Models.TaskSystem.TaskCompletion
            {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                TaskId = dailyCheckinTask.Id,
                CompletedAt = DateTime.UtcNow
            });
            user.Points += dailyCheckinTask.Points;
            await _dbContext.SaveChangesAsync();

            return Ok(new { alreadyCheckedIn = false, message = "Check-in successful! Points awarded." });
        }
    }
}
