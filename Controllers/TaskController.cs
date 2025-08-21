using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shhhtoshi.Api.DB;
using ShhhToshiApp.DTOs.TaskDTOs;
using ShhhToshiApp.Models.TaskSystem;

namespace ShhhToshiApp.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private const int ConversionRate = 100; // 100 points = 1 TON

        public TaskController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST: Add a new task item
        [HttpPost]
        public async Task<IActionResult> AddTaskItem([FromBody] TaskItemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.Points <= 0)
                return BadRequest("Invalid task data");

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                Points = dto.Points,
                IsActive = dto.IsActive
            };

            _dbContext.TaskItems.Add(task);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActiveTasks), new { id = task.Id }, task);
        }

        // GET: Fetch all active tasks for display
        [HttpGet]
        public async Task<IActionResult> GetActiveTasks()
        {
            var tasks = await _dbContext.TaskItems
                .Where(t => t.IsActive)
                .Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Points = t.Points,
                    IsActive = t.IsActive
                })
                .ToListAsync();

            return Ok(tasks);
        }

        // POST: Mark a task as completed and award points
        [HttpPost("complete")]
        public async Task<IActionResult> CompleteTask(
            [FromHeader(Name = "X-Wallet-Address")] string walletAddress,
            [FromBody] TaskCompleteDto dto)
        {
            // Validate wallet existence
            var user = await _dbContext.WalletUsers
                .FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user is null) return NotFound("Wallet not registered");

            // Prevent duplicate completion
            var already = await _dbContext.TaskCompletions
                .AnyAsync(tc => tc.WalletAddress == walletAddress && tc.TaskId == dto.TaskId);
            if (already) return BadRequest("Already completed");

            // Validate task existence
            var task = await _dbContext.TaskItems.FindAsync(dto.TaskId);
            if (task is null) return NotFound("Task not found");

            // Record task completion
            _dbContext.TaskCompletions.Add(new TaskCompletion
            {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                TaskId = dto.TaskId,
                CompletedAt = DateTime.UtcNow
            });

            // Award points to user
            user.Points += task.Points;
            await _dbContext.SaveChangesAsync();

            return Ok(new { user.Points });
        }

        // GET: Return current points and conversion rate
        [HttpGet("/api/points")]
        public async Task<IActionResult> GetPoints(
            [FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            var user = await _dbContext.WalletUsers
                .FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user is null) return NotFound("Wallet not registered");

            var dto = new PointsInfoDto
            {
                Points = user.Points,
                ConversionRate = ConversionRate
            };

            return Ok(dto);
        }

        // POST: Claim all points and convert to TON
        [HttpPost("/api/points/claim")]
        public async Task<IActionResult> ClaimPoints(
            [FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            var user = await _dbContext.WalletUsers
                .FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user is null) return NotFound("Wallet not registered");

            // Ensure user has enough points to claim
            if (user.Points < ConversionRate) return BadRequest("Insufficient points");

            // Convert points to TON
            var converted = user.Points / (decimal)ConversionRate;

            // Record claim history
            _dbContext.PointClaims.Add(new PointClaim
            {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                PointsClaimed = user.Points,
                ConvertedAmount = converted,
                ClaimedAt = DateTime.UtcNow
            });

            // Add converted balance to user wallets ton balance
            user.TONBalance += converted;
            
            // Reset user points after claim
            user.Points = 0;
            await _dbContext.SaveChangesAsync();

            var dto = new PointClaimResponseDto
            {
                ConvertedAmount = converted
            };

            return Ok(dto);
        }

        // GET: Return claim history for the wallet
        [HttpGet("/api/points/history")]
        public async Task<IActionResult> ClaimHistory(
            [FromHeader(Name = "X-Wallet-Address")] string walletAddress)
        {
            var history = await _dbContext.PointClaims
                .Where(pc => pc.WalletAddress == walletAddress)
                .OrderByDescending(pc => pc.ClaimedAt)
                .Select(pc => new ClaimHistoryDto
                {
                    Id = pc.Id,
                    PointsClaimed = pc.PointsClaimed,
                    ConvertedAmount = pc.ConvertedAmount,
                    ClaimedAt = pc.ClaimedAt
                })
                .ToListAsync();

            return Ok(history);
        }
    }
}
