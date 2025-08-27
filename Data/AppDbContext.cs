using Microsoft.EntityFrameworkCore;
using ShhhToshiApp.Models;
using ShhhToshiApp.Models.StakingUnstaking;
using ShhhToshiApp.Models.TaskSystem;

namespace Shhhtoshi.Api.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WalletUser> WalletUsers { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskCompletion> TaskCompletions { get; set; }
    public DbSet<PointClaim> PointClaims { get; set; }
    public DbSet<StakeUnstakeEvent> StakeUnstakeEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WalletUser>()
            .HasIndex(u => u.WalletAddress)
            .IsUnique();

            // Composite relationship for Tasks

            modelBuilder.Entity<TaskCompletion>()
                .HasOne(tc => tc.WalletUser)
                .WithMany(u => u.TaskCompletions)
                .HasForeignKey(tc => tc.WalletAddress)
                .HasPrincipalKey(u => u.WalletAddress);

            modelBuilder.Entity<PointClaim>()
                .HasOne(pc => pc.WalletUser)
                .WithMany(u => u.PointClaims)
                .HasForeignKey(pc => pc.WalletAddress)
                .HasPrincipalKey(u => u.WalletAddress);

            modelBuilder.Entity<TaskCompletion>()
                .HasOne(tc => tc.TaskItem)
                .WithMany()
                .HasForeignKey(tc => tc.TaskId);

            modelBuilder.Entity<StakeUnstakeEvent>()
                .HasOne<WalletUser>()
                .WithMany()
                .HasForeignKey(e => e.WalletAddress)
                .HasPrincipalKey(u => u.WalletAddress);
        }
    }
}
