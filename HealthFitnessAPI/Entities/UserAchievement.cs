using HealthFitnessAPI.Constants.Enums;
namespace HealthFitnessAPI.Entities
{
    public class UserAchievement : AbstractEntity
    {
        public AchievementLevel AchievementLevel { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int AchievementId { get; set; }
        public Achievement? Achievement { get; set; }
    }
}
