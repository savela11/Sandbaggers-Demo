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
    public class IdeaController : ControllerBase
    {
        private readonly IService _service;

        public IdeaController(IService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult> AllIdeas()
        {
            var response = await _service.Idea.AllIdeas();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }



        [HttpPost]
        public async Task<ActionResult> AddIdea(AddIdeaDto addIdeaDto)
        {
            var response = await _service.Idea.AddIdea(addIdeaDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveIdea(int id)
        {
            var response = await _service.Idea.RemoveIdea(id);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateIdea(IdeaVm ideaVm)
        {
            var response = await _service.Idea.UpdateIdea(ideaVm);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
