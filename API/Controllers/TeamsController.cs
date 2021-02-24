using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> TeamsByEvent(int eventId)
        {
            var response = await _teamService.TeamsByEvent(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }






        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateTeamForEvent(CreateTeamForEventDto createTeamForEventDto)
        {
            var response = await _teamService.CreateTeamForEvent(createTeamForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var response = await _teamService.RemoveTeamFromEvent(removeTeamFromEventDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateTeams(List<TeamVm> teamVmList)
        {
            var response = await _teamService.UpdateTeams(teamVmList);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
