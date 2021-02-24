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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateEvent(CreateEventDto createEventDto)
        {
            var response = await _eventService.CreateEvent(createEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> UpdateEvent(EventVm sandbaggerEventVm)
        {
            var response = await _eventService.UpdateEvent(sandbaggerEventVm);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{eventId}")]
        public async Task<ActionResult> EventForEventManager(int eventId)
        {
            var response = await _eventService.EventForEventManager(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> UnRegisterUserFromEvent(RemoveUserFromEventDto removeUserFromEventDto)
        {
            var response = await _eventService.RemoveUserFromEvent(removeUserFromEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> RegisterUserForEvent(RegisterUserForEventDto registerUserForEventDto)
        {
            var response = await _eventService.RegisterUserForEvent(registerUserForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> PublishedEvents()
        {
            var response = await _eventService.PublishedEventsByYear();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{eventId}")]
        public async Task<ActionResult> DeleteEvent(int eventId)
        {
            var response = await _eventService.DeleteEvent(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
