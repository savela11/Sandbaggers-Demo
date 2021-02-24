using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.ViewModels;

using Services.Interface;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetsController(IBetService betService)
        {
            _betService = betService;
        }

        [HttpGet]
        [Route("{betId}")]
        public async Task<ActionResult> Bet(int betId)
        {
            var response = await _betService.Bet(betId);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        public async Task<ActionResult> AllActiveBets()
        {
            var response = await _betService.AllActiveBets();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<ActionResult> CreateBet(CreateBetDto createBetDto)
        {
            var response = await _betService.CreateBet(createBetDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> UserBets(string id)
        {
            var response = await _betService.UserBets(id);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBet(DeleteBetDto deleteBetDto)
        {
            var response = await _betService.DeleteBet(deleteBetDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBet(BetVm betVm)
        {
            var response = await _betService.UpdateBet(betVm);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AcceptBet(UserAcceptedBetDto userAcceptedBetDto)
        {
            var response = await _betService.AcceptBet(userAcceptedBetDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
