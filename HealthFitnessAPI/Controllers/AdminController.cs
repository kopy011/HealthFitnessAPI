using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Model.Dtos.Auth;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.Services;
using HealthFitnessAPI.Services.Init;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(
    IMockDataInitService mockDataInitService,
    IAuthService authService,
    IUserService userService,
    IMapper mapper) : ControllerBase
{
    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpGet("init")]
    public async Task<IActionResult> Init()
    {
        await mockDataInitService.Initialize();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var token = await authService.AdminLogin(loginDto);
            return Ok(token);
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] AdminChangePasswordDto adminChangePassword)
    {
        try
        {
            var user = await userService.AdminChangePassword(adminChangePassword);
            return Ok(mapper.Map<UserResultDto>(user));
        }
        catch (Exception)
        {
            return Unauthorized();
        }
    }
}