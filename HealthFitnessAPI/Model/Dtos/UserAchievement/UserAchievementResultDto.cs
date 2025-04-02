using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.Model.Dtos.User;
namespace HealthFitnessAPI.Model.Dtos.UserAchievement
{
    public class UserAchievementResultDto
    {
        public int Id { get; set; }
        public string AchievementLevel { get; set; }
        public int UserId { get; set; }
        public UserResultDto User { get; set; }
        public int AchievementId { get; set; }
        public AchievementResultDto Achievement { get; set; }
    }
}
