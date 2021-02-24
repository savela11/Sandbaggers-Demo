using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventResultsController : ControllerBase
    {
        private readonly IEventResultsService _eventResultsService;

        public EventResultsController(IEventResultsService eventResultsService)
        {
            _eventResultsService = eventResultsService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Results(int id)
        {
            var response = await _eventResultsService.EventResults(id);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> ScrambleChamps()
        {
            var response = await _eventResultsService.ScrambleChamps();
            if (response.Success == false)
            {
                if (response.Message == "No Scramble Champs set")
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(response);
                }
            }

            return Ok(response.Data);
        }


    }
}
