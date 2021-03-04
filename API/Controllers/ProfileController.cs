using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Services.Interface;

namespace API.Controllers
{
    // public class UploadProfileImage
    // {
    //     public string Name { get; set; }
    //     public string Container { get; set; }
    //     public IFormFile Image { get; set; }
    //     public IFormCollection FormData { get; set; }
    //     public string Link { get; set; }
    // }

    // [Authorize]
    // [ApiController]
    // [Route("api/[controller]/[action]")]
    // public class ProfileController : ControllerBase
    // {
    //     private readonly IProfileService _profileService;
    //     private readonly IAzureStorageService _azureStorageService;
    //
    //     public ProfileController(IProfileService profileService, IAzureStorageService azureStorageService)
    //     {
    //         _profileService = profileService;
    //         _azureStorageService = azureStorageService;
    //     }
    //
    //     [HttpPost]
    //     public async Task<ActionResult> UpdateUser(UserVm userVm)
    //     {
    //         var response = await _profileService.UpdateUser(userVm);
    //         if (response.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         return Ok(response.Data);
    //     }
    //
    //
    //     [HttpGet("{userId}")]
    //     public async Task<ActionResult> UserProfile(string userId)
    //     {
    //         var response = await _profileService.UserProfile(userId);
    //         if (response.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         return Ok(response.Data);
    //     }
    //
    //     [HttpPut]
    //     public async Task<ActionResult> UpdateBet(BetVm betVm)
    //     {
    //         var response = await _profileService.UpdateBet(betVm);
    //         if (response.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         return Ok(response.Data);
    //     }
    //
    //     [HttpPost]
    //     public async Task<ActionResult> DeleteBet(BetVm betVm)
    //     {
    //         var response = await _profileService.DeleteBet(betVm);
    //         if (response.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         return Ok(response.Data);
    //     }
    //
    //     [HttpPost]
    //     public async Task<ActionResult> UploadProfileImage(IFormCollection formCollection)
    //     {
    //         var response = await _azureStorageService.UploadImage(formCollection, "profileimg");
    //
    //         if (response.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         var userId = formCollection["userId"].ToString();
    //         var profileServiceResponse = await _profileService.UpdateProfileImage(userId, response.Data);
    //         if (profileServiceResponse.Success == false)
    //         {
    //             return BadRequest(response);
    //         }
    //
    //         return Ok(profileServiceResponse.Data);
    //     }
    // }
}
