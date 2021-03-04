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
        private readonly IService _service;

        public TeamManagerController(IService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<ActionResult> CreateTeam(CreateTeamForEventDto createTeamForEventDto)
        {
            var response = await _service.Team.CreateTeamForEvent(createTeamForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var response = await _service.Team.RemoveTeamFromEvent(removeTeamFromEventDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            var eventVmForManagerResponse = await _service.Event.EventForEventManager(response.Data.EventId);
            if (eventVmForManagerResponse.Success == false)
            {
                return BadRequest(eventVmForManagerResponse);
            }

            return Ok(eventVmForManagerResponse.Data);
        }


        [HttpGet("{eventId}")]
        public async Task<ActionResult> Team(int eventId)
        {
            var response = await _service.Team.TeamsByEvent(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
