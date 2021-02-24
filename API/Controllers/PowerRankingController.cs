using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PowerRankingController : ControllerBase
    {
        private readonly IPowerRankingService _powerRankingService;

        public PowerRankingController(IPowerRankingService powerRankingService)
        {
            _powerRankingService = powerRankingService;
        }

        [HttpGet]
        public async Task<ActionResult> PowerRankings()
        {
            var response = await _powerRankingService.PowerRankings();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> PowerRankingById(int eventId)
        {
            var response = await _powerRankingService.PowerRanking(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserRanking(CreateRankingDto createRankingDto)
        {
            var response = await _powerRankingService.CreateUserRanking(createRankingDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> EventRegisteredUsers(int eventId)
        {
            var response = await _powerRankingService.EventRegisteredUsers(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
