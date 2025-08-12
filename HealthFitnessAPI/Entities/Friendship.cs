using System.ComponentModel.DataAnnotations.Schema;
using HealthFitnessAPI.Constants.Enums;

namespace HealthFitnessAPI.Entities;

public class Friendship
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.FriendsSent))]
    public User User { get; set; }

    public int FriendId { get; set; }

    [ForeignKey(nameof(FriendId))]
    [InverseProperty(nameof(User.FriendsRecieved))]
    public User Friend { get; set; }

    public FriendshipStatus Status { get; set; }

    public DateTime LastUpdated { get; set; }
}