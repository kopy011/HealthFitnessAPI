using HealthFitnessAPI.Constants;

namespace HealthFitnessAPI.Extensions;

public static class HttpContextExtensions
{
    public static int GetUserIdOrThrow(this HttpContext httpContext)
    {
        int? userId = null;
        if (httpContext.Items.TryGetValue(RequestKeys.UserId, out var userIdObj) &&
            int.TryParse(userIdObj?.ToString(), out var parsedUserId))
            userId = parsedUserId;

        return userId ?? throw new Exception("UserId is required.");
    }
}