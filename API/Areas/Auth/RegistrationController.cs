using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Auth
{

    [Area("Auth")]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]

    public class RegistrationController : ControllerBase
    {
            private readonly IAuthService _authService;

                public RegistrationController(IAuthService authService)
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
    }
}
