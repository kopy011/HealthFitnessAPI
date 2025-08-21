using HealthFitnessAPI.Model.Dtos.Auth;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await authService.Login(loginDto);
            return Ok(token);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
    {
        try
        {
            var result = await authService.ValidateRefreshToken(refreshDto.RefreshToken!);
            return Ok(result);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Revoke([FromBody] RefreshDto refreshDto)
    {
        try
        {
            await authService.RevokeRefreshToken(refreshDto.RefreshToken!);
            return Ok();
        }
        catch (Exception)
        {
            return Ok();
        }
    }
}