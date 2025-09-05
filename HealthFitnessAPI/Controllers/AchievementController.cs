using System.Web;
using AutoMapper;
using AutoWrapper.Filters;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AchievementController(IAchievementService achievementService, IMapper mapper, IFileService fileService)
    : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> GetAll()
    {
        var achievements = await achievementService.GetAllWithImages();
        return Ok(achievements);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> GetById(int id)
    {
        var achievement = await achievementService.GetById(id);
        return Ok(mapper.Map<AchievementResultDto>(achievement));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Create([FromBody] CreateAchievementDto achievement)
    {
        try
        {
            var result = await achievementService.CreateWithUpload(achievement);
            return Ok(mapper.Map<AchievementResultDto>(result));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Update([FromBody] UpdateAchievementDto achievement)
    {
        try
        {
            var result = await achievementService.UpdateWithUpload(achievement);
            return Ok(mapper.Map<AchievementResultDto>(result));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await achievementService.DeleteSoft(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("image/{imageName}")]
    [AllowAnonymous]
    [AutoWrapIgnore]
    public IActionResult GetBadge(string imageName)
    {
        var fs = fileService.GetFileStream(HttpUtility.UrlDecode(imageName));
        return File(fs, "application/octet-stream", true);
    }
}