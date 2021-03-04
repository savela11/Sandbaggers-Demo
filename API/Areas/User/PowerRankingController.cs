using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.User
{

    [Area("User")]
    [Authorize(Policy = "User")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class PowerRankingController : ControllerBase
    {
          private readonly IService _service;

          public PowerRankingController(IService service)
          {
              _service = service;
          }


               [HttpGet]
                  public async Task<ActionResult> List()
                  {
                      var response = await _service.PowerRanking.PowerRankings();
                      if (response.Success == false)
                      {
                          return BadRequest(response);
                      }

                      return Ok(response.Data);
                  }

                  [HttpGet("{eventId}")]
                  public async Task<ActionResult> ById(int eventId)
                  {
                      var response = await _service.PowerRanking.PowerRanking(eventId);
                      if (response.Success == false)
                      {
                          return BadRequest(response);
                      }

                      return Ok(response.Data);
                  }



                  [HttpGet("{eventId}")]
                  public async Task<ActionResult> RegisteredUsersByEventId(int eventId)
                  {
                      var response = await _service.PowerRanking.EventRegisteredUsers(eventId);
                      if (response.Success == false)
                      {
                          return BadRequest(response);
                      }

                      return Ok(response.Data);
                  }
    }
}
