using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace API.Areas.User;

[Area("User")]
[Authorize(Policy = "User")]
[ApiController]
[Route("api/[area]/[controller]/[action]")]
public class BranchInformationController : ControllerBase
{
  private readonly IBranchInfoService _branchInfoService;

  public BranchInformationController(IBranchInfoService branchInfoService)
  {
    _branchInfoService = branchInfoService;
  }


  public async Task<ActionResult> GetBranches()
  {
    var response = await _branchInfoService.BranchInfoList();

    if (response.Success == false)
    {
      return BadRequest(response);
    }


    return Ok(response.Data);
  }


  public async Task<ActionResult> GetBranchIds()
  {
    var response = await _branchInfoService.BranchInfoList();

    if (response.Success == false)
    {
      return BadRequest(response);
    }

    var branchIds = response.Data.Select(b => b.Id).ToList();


    return Ok(branchIds);
  }
}