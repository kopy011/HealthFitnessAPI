using AutoMapper;
using HealthFitnessAPI.Entities;
using HealthFitnessAPI.Model.Dtos.Achievement;
using HealthFitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;
namespace HealthFitnessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AchievementController(IAchievementService achievementService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var achievements = await achievementService.GetAll();
            return Ok(mapper.Map<List<AchievementResultDto>>(achievements));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var achievement = await achievementService.GetById(id);
            return Ok(mapper.Map<AchievementResultDto>(achievement));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAchievementDto achievement)
        {
            try
            {
                var result = await achievementService.Create(mapper.Map<Achievement>(achievement));
                return Ok(mapper.Map<AchievementResultDto>(result));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateAchievementDto achievement)
        {
            try
            {
                var result = await achievementService.Update(mapper.Map<Achievement>(achievement));
                return Ok(mapper.Map<AchievementResultDto>(result));
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
                await achievementService.DeleteSoft(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
