using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Services.Init;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IMockDataInitService mockDataInitService) : ControllerBase
{
    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpGet("init")]
    public async Task<IActionResult> Init()
    {
        await mockDataInitService.Initialize();
        return Ok();
    }
}