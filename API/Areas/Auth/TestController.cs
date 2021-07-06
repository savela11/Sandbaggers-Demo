using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Areas.Auth
{
    [Area("Auth")]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly IService _service;

        public TestController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> AllUsers()
        {
            var result = await _service.User.Users();
            if (result.Success == false)
            {
                return BadRequest(result);
            }

            return Ok(result.Data);
        }
    }
}
