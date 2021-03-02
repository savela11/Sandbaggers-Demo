using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;

namespace API.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class EventManagerController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventManagerController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> Event(int eventId)
        {
            var response = await _eventService.EventForEventManager(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

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

        [HttpPost]
        public async Task<ActionResult> UnRegisterUser(RemoveUserFromEventDto removeUserFromEventDto)
        {
            var response = await _eventService.RemoveUserFromEvent(removeUserFromEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(RegisterUserForEventDto registerUserForEventDto)
        {
            var response = await _eventService.RegisterUserForEvent(registerUserForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
