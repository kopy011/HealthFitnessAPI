using HealthFitnessAPI.Model.Dtos.UserAchievement;

namespace HealthFitnessAPI.Model.Dtos.User;

public class PublicProfileDto
{
    public UserResultDto User { get; set; }
    public List<UserAchievementWithAchievementDto> UserAchievements { get; set; }
}