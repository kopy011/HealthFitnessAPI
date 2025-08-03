using HealthFitnessAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace HealthFitnessAPI.Context
{
    public class HealthFitnessDbContext(DbContextOptions<HealthFitnessDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted).HasDefaultValue(false);
            modelBuilder.Entity<Achievement>().HasQueryFilter(a => !a.Deleted).Property(a => a.Deleted).HasDefaultValue(false);
            modelBuilder.Entity<UserAchievement>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted).HasDefaultValue(false);
            modelBuilder.Entity<RefreshToken>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted).HasDefaultValue(false);
        }
    }
}
