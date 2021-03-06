using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;

namespace API.Areas.User
{
    [Area("User")]
    [Authorize(Policy = "User")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class BetController : ControllerBase
    {
        // private readonly IService _service;
        private readonly IBetService _betService;

        public BetController(IBetService betService)
        {
          _betService = betService;
        }

        [HttpGet]
        [Route("{betId}")]
        public async Task<ActionResult> ById(int betId)
        {
            var response = await _betService.BetById(betId);
            
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            var betVmResponse = await _betService.BetVm(response.Data);

            if (betVmResponse.Success == false)
            {
                return BadRequest(betVmResponse);
            }


            return Ok(betVmResponse.Data);
        }

        [HttpGet]
        [Route("{betId}")]
        public async Task<ActionResult> BetVmById(int betId)
        {
            var response = await _service.Bet.BetVmById(betId);
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
        public async Task<ActionResult> ByUserId(string id)
        {
            var response = await _service.Bet.UserBets(id);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteBet(DeleteBetDto deleteBetDto)
        {
            var response = await _service.Bet.DeleteBet(deleteBetDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBet(BetVm betVm)
        {
            var response = await _service.Bet.UpdateBet(betVm);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AcceptBet(UserAcceptedBetDto userAcceptedBetDto)
        {
            var response = await _service.Bet.AcceptBet(userAcceptedBetDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> MyBets(string userId)
        {
            var response = await _service.Bet.MyBets(userId);
            if (response.Success == false) return BadRequest(response);

            return Ok(response.Data);
        }
    }
}
