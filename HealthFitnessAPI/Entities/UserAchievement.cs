namespace HealthFitnessAPI.Entities;

public class UserAchievement : AbstractEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int AchievementId { get; set; }
    public Achievement? Achievement { get; set; }

    public int AchievementLevelId { get; set; }
    public AchievementLevel? AchievementLevel { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<UserAchievementLike> Likes { get; set; }
}