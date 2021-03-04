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
    public class ContactController : ControllerBase
    {
        private readonly IService _service;

        public ContactController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> List()
        {
            var contactResponse = await _service.Contact.Contacts();
            if (contactResponse.Success == false)
            {
                return BadRequest(contactResponse);
            }

            var contactVmListResponse = await _service.Contact.ContactVmList(contactResponse.Data);

            if (contactVmListResponse.Success == false)
            {
                return BadRequest(contactVmListResponse);
            }

            return Ok(contactVmListResponse.Data);
        }
    }
}
