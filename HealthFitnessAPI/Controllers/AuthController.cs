using HealthFitnessAPI.Model.Dtos.Auth;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await authService.Login(loginDto);
        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
    {
        var result = await authService.ValidateRefreshToken(refreshDto.Token!);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Revoke([FromBody] RefreshDto refreshDto)
    {
        await authService.RevokeRefreshToken(refreshDto.Token!);
        return Ok();
    }
}