using HealthFitnessAPI.Model.Dtos.User;

namespace HealthFitnessAPI.Model.Dtos.Friendship;

public class AbstractFriendshipDto
{
    public UserResultDto User { get; set; }
    public UserResultDto Friend { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdated { get; set; }
}