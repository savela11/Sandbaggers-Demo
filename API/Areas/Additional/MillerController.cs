using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Additional
{
    [Area("User")]
    [Authorize(Policy = "Miller")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class MillerController : ControllerBase
    {
        private readonly IService _service;

        public MillerController(IService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserRanking(CreateRankingDto createRankingDto)
        {
            var response = await _service.PowerRanking.CreateUserRanking(createRankingDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
