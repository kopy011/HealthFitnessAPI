using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.UserAchievement;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAchievementController(IUserAchievementService userAchievementService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> GetAll()
    {
        var userAchievements = await userAchievementService.GetAllWithInclude();
        return Ok(mapper.Map<List<UserAchievementResultDto>>(userAchievements));
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userAchievement = await userAchievementService.GetById(id);
        return Ok(mapper.Map<UserAchievementResultDto>(userAchievement));
    }

    [HttpGet("user/{id:int}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.User}")]
    public async Task<IActionResult> GetByUserId(int id)
    {
        var userAchievement = await userAchievementService.GetAllByUserId(id);
        return Ok(mapper.Map<List<UserAchievementResultDto>>(userAchievement));
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Create([FromBody] CreateUserAchievementDto userAchievement)
    {
        try
        {
            var result = await userAchievementService.Create(mapper.Map<UserAchievement>(userAchievement));
            return Ok(mapper.Map<UserAchievementResultDto>(result));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.Admin}")]
    public async Task<IActionResult> Update([FromBody] UpdateUserAchievementDto userAchievement)
    {
        try
        {
            var result = await userAchievementService.Update(mapper.Map<UserAchievement>(userAchievement));
            return Ok(mapper.Map<UserAchievementResultDto>(result));
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
            await userAchievementService.DeleteSoft(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}