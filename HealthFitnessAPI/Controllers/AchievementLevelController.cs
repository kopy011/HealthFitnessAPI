using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Entities;
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
        var result = await achievementLevelService.GetAll();
        return Ok(mapper.Map<List<AchievementLevelResultDto>>(result));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Create([FromBody] CreateAchievementLevelDto createAchievementLevelDto)
    {
        var result = await achievementLevelService.Create(mapper.Map<AchievementLevel>(createAchievementLevelDto));
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Update([FromBody] UpdateAchievementLevelDto updateAchievementLevelDto)
    {
        var result = await achievementLevelService.Update(mapper.Map<AchievementLevel>(updateAchievementLevelDto));
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