using System.Linq;
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
    public class DashboardController : ControllerBase
    {
        private readonly IService _service;

        public DashboardController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> DashboardData()
        {
            var response = await _service.Dashboard.DashboardData();
            if (response.Success == false)
            {
                return BadRequest(response);
            }


            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> ActiveBets()
        {
            var response = await _service.Bet.AllActiveBets();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            var betVmListResponse = await _service.Bet.BetVmList(response.Data);
            if (betVmListResponse.Success == false)
            {
                return BadRequest(betVmListResponse);
            }


            return Ok(betVmListResponse.Data);
        }
    }
}
