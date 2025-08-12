using System.IdentityModel.Tokens.Jwt;
using HealthFitnessAPI.Constants;

namespace HealthFitnessAPI.Middlewares;

public sealed class UserIdMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var rawToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CanReadToken(rawToken) ? handler.ReadJwtToken(rawToken) : null;
        var userId = token?.Claims.FirstOrDefault(claim => claim.Type == "nameid")?.Value;

        if (userId != null) context.Items[RequestKeys.UserId] = userId;

        return next(context);
    }
}