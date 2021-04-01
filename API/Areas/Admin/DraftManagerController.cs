using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Models.ViewModels.Views;
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
            var response = await _service.Draft.AdminDraftManagerData();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> EditDraft(DraftManagerViewData draftManagerViewData)
        {
            var response = await _service.Draft.EditDraft(draftManagerViewData);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


    }
}
