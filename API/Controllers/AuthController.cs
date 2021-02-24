using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;


namespace API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {


            var response = await _authService.RegisterUser(registerUserDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response.Data);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterMultipleUsers(List<RegisterUserDto> registerUserDtoList)
        {
            var response = await _authService.RegisterMultipleUsers(registerUserDtoList);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
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
