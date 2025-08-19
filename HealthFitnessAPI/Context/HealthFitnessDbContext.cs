using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Constants.Enums;
using HealthFitnessAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthFitnessAPI.Context;

public class HealthFitnessDbContext(DbContextOptions<HealthFitnessDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AchievementLevel> AchievementLevels { get; set; }
    public DbSet<AchievementLevelThreshold> AchievementLevelThresholds { get; set; }
    public DbSet<UserAchievement> UserAchievements { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Friendship> Friendships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted).HasDefaultValue(false);
        modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue(Roles.User);
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Friendship>().HasKey(u => new { u.UserId, u.FriendId });
        modelBuilder.Entity<Friendship>().Property(u => u.Status).HasDefaultValue(FriendshipStatus.Pending);
        modelBuilder.Entity<Friendship>().HasIndex(u => u.UserId).HasDatabaseName("IX_Friendship_UserId");
        modelBuilder.Entity<Friendship>().HasIndex(u => u.FriendId).HasDatabaseName("IX_Friendship_FriendId");


        modelBuilder.Entity<Friendship>().HasOne(f => f.User)
            .WithMany(u => u.FriendsSent)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Friendship>().HasOne(f => f.Friend)
            .WithMany(u => u.FriendsRecieved)
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Achievement>().HasQueryFilter(a => !a.Deleted).Property(a => a.Deleted)
            .HasDefaultValue(false);
        modelBuilder.Entity<Achievement>().HasIndex(a => a.Category).IsUnique();

        modelBuilder.Entity<AchievementLevel>().HasQueryFilter(a => !a.Deleted).Property(a => a.Deleted)
            .HasDefaultValue(false);
        modelBuilder.Entity<AchievementLevel>().HasIndex(a => a.Name).IsUnique();

        modelBuilder.Entity<AchievementLevelThreshold>().HasQueryFilter(a => !a.Deleted).Property(a => a.Deleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<UserAchievement>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted)
            .HasDefaultValue(false);

        modelBuilder.Entity<RefreshToken>().HasQueryFilter(u => !u.Deleted).Property(u => u.Deleted)
            .HasDefaultValue(false);
    }
}