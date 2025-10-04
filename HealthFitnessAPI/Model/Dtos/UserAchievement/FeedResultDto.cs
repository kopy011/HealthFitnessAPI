namespace HealthFitnessAPI.Model.Dtos.UserAchievement;

public class FeedResultDto
{
    public int TotalCount { get; set; }
    public List<UserAchievementResultDto> UserAchievements { get; set; }
}