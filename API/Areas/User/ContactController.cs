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
        public async Task<ActionResult> ContactList()
        {
            var response = await _service.Contact.ContactVmList();
            if (response.Success == false)
            {
                return BadRequest(response);
            }


            return Ok(response.Data);
        }
    }
}
