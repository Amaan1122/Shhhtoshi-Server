using Microsoft.EntityFrameworkCore;
using Shhhtoshi.Api.DB;
using ShhhToshiApp.Models.TaskSystem;

namespace ShhhToshiApp.Services
{
    public class TaskCompletionService
    {
        private readonly AppDbContext _dbContext;
        public TaskCompletionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Checks if the user has completed the given task, and if not, marks it as completed and awards points.
        /// </summary>
        /// <param name="walletAddress">User's wallet address</param>
        /// <param name="taskTitle">Title of the task (must be unique)</param>
        /// <returns>True if task was completed now, false if already completed or not found</returns>
        public async Task<bool> CompleteTaskIfNotAlready(string walletAddress, string taskTitle)
        {
            var user = await _dbContext.WalletUsers.FirstOrDefaultAsync(u => u.WalletAddress == walletAddress);
            if (user == null) return false;

            var task = await _dbContext.TaskItems.FirstOrDefaultAsync(t => t.Title == taskTitle);
            if (task == null) return false;

            var alreadyCompleted = await _dbContext.TaskCompletions.AnyAsync(tc => tc.WalletAddress == walletAddress && tc.TaskId == task.Id);
            if (alreadyCompleted) return false;

            _dbContext.TaskCompletions.Add(new TaskCompletion
            {
                Id = Guid.NewGuid(),
                WalletAddress = walletAddress,
                TaskId = task.Id,
                CompletedAt = DateTime.UtcNow
            });
            user.Points += task.Points;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
