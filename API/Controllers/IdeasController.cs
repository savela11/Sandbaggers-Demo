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
    public class IdeasController : ControllerBase
    {
        private readonly IIdeaService _ideaService;

        public IdeasController(IIdeaService ideaService)
        {
            _ideaService = ideaService;
        }

        [HttpGet]
        public async Task<ActionResult> AllIdeas()
        {
            var response = await _ideaService.AllIdeas();
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Idea(GetIdeaDto getIdeaDto)
        {
            var response = await _ideaService.Idea(getIdeaDto);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<ActionResult> AddIdea(AddIdeaDto addIdeaDto)
        {
            var response = await _ideaService.AddIdea(addIdeaDto);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveIdea(int id)
        {
            var response = await _ideaService.RemoveIdea(id);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateIdea(IdeaVm ideaVm)
        {
            var response = await _ideaService.UpdateIdea(ideaVm);
            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response.Data);
        }
    }
}
