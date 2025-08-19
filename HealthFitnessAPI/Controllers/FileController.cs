using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.Admin}, {Roles.User}")]
public class FileController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFiles([FromServices] IFileService fileService)
    {
        var files = await fileService.GetAllFilesAsync();
        return Ok(files);
    }
}