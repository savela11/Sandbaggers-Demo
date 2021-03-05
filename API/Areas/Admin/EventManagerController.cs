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
        private readonly IService _service;

        public EventManagerController( IService service)
        {
            _service = service;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult> Event(int eventId)
        {
            var response = await _service.Event.EventForEventManager(eventId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> EventList()
        {
            var response = await _service.Event.Events();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            var eventVmListResponse = await _service.Event.EventVmList(response.Data);

            return Ok(eventVmListResponse.Data);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEvent(CreateEventDto createEventDto)
        {
            var response = await _service.Event.CreateEvent(createEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateEvent(EventVm sandbaggerEventVm)
        {
            var eventResponse = await _service.Event.UpdateEvent(sandbaggerEventVm);
            if (eventResponse.Success == false)
            {
                return BadRequest(eventResponse);
            }

            var eventVmResponse = await _service.Event.EventVm(eventResponse.Data);

            if (eventVmResponse.Success == false)
            {
                return BadRequest(eventVmResponse);
            }

            return Ok(eventVmResponse.Data);
        }

        [HttpPost]
        public async Task<ActionResult> UnRegisterUser(RemoveUserFromEventDto removeUserFromEventDto)
        {
            var response = await _service.Event.RemoveUserFromEvent(removeUserFromEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(RegisterUserForEventDto registerUserForEventDto)
        {
            var response = await _service.Event.RegisterUserForEvent(registerUserForEventDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
