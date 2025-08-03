using AutoMapper;
using HealthFitnessAPI.Constants;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthFitnessAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll();
        return Ok(mapper.Map<List<UserResultDto>>(users));
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await userService.GetById(id);
        return Ok(mapper.Map<UserResultDto>(user));
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto user)
    {
        try
        {
            var result = await userService.Create(mapper.Map<User>(user));
            return Ok(mapper.Map<UserResultDto>(result));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto user)
    {
        try
        {
            var result = await userService.Update(mapper.Map<User>(user));
            return Ok(mapper.Map<UserResultDto>(result));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.User}")]
    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto user)
    {
        var result = await userService.ChangePassword(user);
        return Ok(mapper.Map<UserResultDto>(result));
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await userService.DeleteSoft(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}