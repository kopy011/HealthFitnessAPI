using HealthFitnessAPI.Model.Dtos.Auth;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController(IAuthService authService): ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token =  await authService.Login(loginDto);
        return Ok(token);
    }
}