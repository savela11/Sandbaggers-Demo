using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class UserManagerController : ControllerBase
    {
        private readonly IService _service;

        public UserManagerController(IService service)
        {
            _service = service;
        }


        public async Task<ActionResult> AllUsers()
        {
            var response = await _service.User.Users();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}