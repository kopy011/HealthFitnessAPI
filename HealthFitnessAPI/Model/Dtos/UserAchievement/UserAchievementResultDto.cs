using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.Model.Dtos.User;
namespace HealthFitnessAPI.Model.Dtos.UserAchievement
{
    public class UserAchievementResultDto
    {
        public int Id { get; set; }
        public int AchievementLevel { get; set; }
        public UserResultDto User { get; set; }
        public AchievementResultDto Achievement { get; set; }
    }
}
