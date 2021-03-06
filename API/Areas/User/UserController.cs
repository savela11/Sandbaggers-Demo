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
    public class UserController : ControllerBase
    {
        private readonly IService _service;

        public UserController( IService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> ById(string id)
        {
            var response = await _service.User.FindUserById(id);
            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }
    }
}
