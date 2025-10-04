using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;
using HealthFitnessAPI.Model.Dtos.User;

namespace HealthFitnessAPI.Model.Dtos.UserAchievement;

public class UserAchievementWithAchievementDto
{
    public int Id { get; set; }
    public int AchievementLevelId { get; set; }
    public string AchievementLevelName { get; set; }
    public int AchievementId { get; set; }
    public AchievementPathOnlyResultDto Achievement { get; set; }
    public AchievementLevelResultDto AchievementLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalLikes { get; set; }
    public bool IsLikedByUser { get; set; }
}

public class UserAchievementResultDto : UserAchievementWithAchievementDto
{
    public int UserId { get; set; }
    public UserResultDto User { get; set; }
}