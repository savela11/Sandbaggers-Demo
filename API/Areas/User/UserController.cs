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
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> ById(string id)
        {
            var response = await _userService.FindUserById(id);
            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }
    }
}
