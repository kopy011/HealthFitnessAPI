using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Extensions;
using HealthFitnessAPI.Model.Dtos.Friendship;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FriendshipController(IUserService userService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> GetFriends()
    {
        var userId = HttpContext.GetUserIdOrThrow();
        var result = await userService.GetFriends(userId);
        return Ok(mapper.Map<List<UserResultDto>>(result));
    }

    [HttpGet("pending/sent")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> GetSentPendingFriendRequests()
    {
        var userId = HttpContext.GetUserIdOrThrow();
        var result = await userService.GetSentPendingFriendRequests(userId);
        return Ok(mapper.Map<List<FriendshipResultDto>>(result));
    }

    [HttpGet("pending/received")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> GetReceivedPendingFriendRequests()
    {
        var userId = HttpContext.GetUserIdOrThrow();
        var result = await userService.GetReceivedPendingFriendRequests(userId);
        return Ok(mapper.Map<List<FriendshipResultDto>>(result));
    }

    [HttpPost("add/{friendId:int}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> AddFriend(int friendId)
    {
        var userId = HttpContext.GetUserIdOrThrow();
        await userService.AddFriend(userId, friendId);
        return Ok();
    }

    [HttpDelete("remove/{friendId:int}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> RemoveFriend(int friendId)
    {
        var userId = HttpContext.GetUserIdOrThrow();
        await userService.RemoveFriend(userId, friendId);
        return Ok();
    }

    [HttpPost("accept/{friendId:int}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> AcceptFriendRequest(int friendId)
    {
        var userId = HttpContext.GetUserIdOrThrow();
        await userService.AcceptFriendRequest(userId, friendId);
        return Ok();
    }

    [HttpDelete("decline/{friendId:int}")]
    [Authorize(Roles = $"{Roles.User}")]
    public async Task<IActionResult> DeclineFriendRequest(int friendId)
    {
        var userId = HttpContext.GetUserIdOrThrow();
        await userService.DeclineFriendRequest(userId, friendId);
        return Ok();
    }
}