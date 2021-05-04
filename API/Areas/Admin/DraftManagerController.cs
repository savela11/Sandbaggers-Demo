using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.Interface;

namespace API.Areas.Admin
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class DraftManagerController : ControllerBase
    {
        private readonly IService _service;

        public DraftManagerController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> AdminDraftManagerData()
        {
            var response = await _service.DraftManager.AdminDraftManagerData();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDraftStatus(UpdateDraftStatusDto draftStatusDto)
        {
            var response = await _service.DraftManager.UpdateDraftStatus(draftStatusDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
