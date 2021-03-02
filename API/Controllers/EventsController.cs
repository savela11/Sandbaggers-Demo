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
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult> EventList()
        {
            var response = await _eventService.Events();
            if (response.Success == false)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetEventById(int id)
        {
            var response = await _eventService.GetEventById(id);
            if (response.Success == false || response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }


    }
}
