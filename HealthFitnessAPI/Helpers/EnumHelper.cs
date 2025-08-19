using HealthFitnessAPI.Constants.Enums;

namespace HealthFitnessAPI.Helpers;

public static class EnumHelper
{
    public static string GetGenderDisplayValue(Gender gender)
    {
        return gender switch
        {
            Gender.Man => "Férfi",
            Gender.Woman => "Nő",
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };
    }

    public static Gender GetGenderEnumValue(string gender)
    {
        return gender switch
        {
            "Férfi" => Gender.Man,
            "Nő" => Gender.Woman,
            _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
        };
    }

    public static string GetFriendShipStatusDisplayValue(FriendshipStatus friendshipStatus)
    {
        return friendshipStatus switch
        {
            FriendshipStatus.Pending => "Pending",
            FriendshipStatus.Accepted => "Accepted",
            _ => throw new ArgumentOutOfRangeException(nameof(friendshipStatus), friendshipStatus, null)
        };
    }

    public static FeedOrderBy GetFeedOrderByEnumValue(string feedOrderBy)
    {
        return feedOrderBy switch
        {
            "Trending" => FeedOrderBy.Trending,
            "DateDescending" => FeedOrderBy.DateDescending,
            "DateAscending" => FeedOrderBy.DateAscending,
            "AToZ" => FeedOrderBy.AToZ,
            "ZToA" => FeedOrderBy.ZToA,
            _ => throw new ArgumentOutOfRangeException(nameof(feedOrderBy), feedOrderBy, null)
        };
    }
}