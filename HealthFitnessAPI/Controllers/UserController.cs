using System.Net;
using System.Net.Mail;
using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.User;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;
namespace HealthFitnessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController(IUserService userService, IMapper mapper, IEmailService emailService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> TestEmail()
        {

            await emailService.SendEmailAsync("lobof73360@kloudis.com", "TEST", "THIS IS A TEST EMAIL");
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAll();
            return Ok(mapper.Map<List<UserResultDto>>(users));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userService.GetById(id);
            return Ok(mapper.Map<UserResultDto>(user));
        }

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
}
