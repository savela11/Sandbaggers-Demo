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
    public class EventsController : ControllerBase
    {
        private readonly IService _service;

        public EventsController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> Published()
        {
            var response = await _service.Event.PublishedEventsByYear();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            var eventVmResponse = await _service.Event.EventVmList(response.Data);
            if (eventVmResponse.Success == false)
            {
                return BadRequest(eventVmResponse);
            }

            return Ok(eventVmResponse.Data);
        }
    }
}
