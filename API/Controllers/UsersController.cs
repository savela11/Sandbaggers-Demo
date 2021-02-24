using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.Interface;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var response = await _userService.Users();
            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var response = await _userService.FindUserById(id);
            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }

        // [HttpGet]
        // [Route("{id}")]
        // public async Task<ActionResult> GetUserByProfileId(string id)
        // {
        //     var profileId = int.Parse(id);
        //     var response = await _userService.FindUserByProfileId(profileId);
        //     if (response.Success == false)
        //     {
        //         return NotFound(response);
        //     }
        //
        //     return Ok(response.Data);
        // }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUserWithSettings(string id)
        {
            var response = await _userService.GetUserWithSettings(id);
            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response.Data);
        }
    }
}
