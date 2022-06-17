using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Areas.User
{

    [Area("User")]
    [Authorize(Policy = "User")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class EventResultController : ControllerBase
    {
        private readonly IService _service;

        public EventResultController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> Results(int id)
        {
            var response = await _service.EventResult.EventResults(id);
            
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> ScrambleChamps()
        {
            var response = await _service.EventResult.ScrambleChamps();
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
