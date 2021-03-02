using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class TeamManagerController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamManagerController(ITeamService teamService)
        {
            _teamService = teamService;
        }


        [HttpPost]
        public async Task<ActionResult> CreateTeam(CreateTeamForEventDto createTeamForEventDto)
        {
            var response = await _teamService.CreateTeamForEvent(createTeamForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var response = await _teamService.RemoveTeamFromEvent(removeTeamFromEventDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpGet("{eventId}")]
        public async Task<ActionResult> Team(int eventId)
        {
            var response = await _teamService.TeamsByEvent(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
