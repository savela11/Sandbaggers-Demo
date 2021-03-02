using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Auth
{

    [Area("Auth")]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        public async Task<ActionResult> UserLogin(LoginUserDto loginUserDto)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var response = await _authService.LoginUser(loginUserDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            var response = await _authService.LogoutUser();

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
