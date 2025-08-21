using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Model.Dtos.AchievementLevel;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/achievement-level")]
public class AchievementLevelController(IAchievementLevelService achievementLevelService, IMapper mapper)
    : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> GetAll()
    {
        var result = await achievementLevelService.GetAllWithImages();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Create([FromBody] CreateAchievementLevelDto createAchievementLevelDto)
    {
        var result = await achievementLevelService.CreateWithUpload(createAchievementLevelDto);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Update([FromBody] UpdateAchievementLevelDto createAchievementLevelDto)
    {
        var result = await achievementLevelService.UpdateWithUpload(createAchievementLevelDto);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Delete(int id)
    {
        await achievementLevelService.DeleteSoft(id);
        return Ok();
    }
}