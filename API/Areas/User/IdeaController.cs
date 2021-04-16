using System;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace API.Areas.User
{
    [Area("User")]
    [Authorize(Policy = "User")]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public class IdeaController : ControllerBase
    {
        private readonly IService _service;
        private readonly AppDbContext _dbContext;

        public IdeaController(IService service, AppDbContext dbContext)
        {
            _service = service;
            _dbContext = dbContext;
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
        public async Task<ActionResult> Idea(GetIdeaDto getIdeaDto)
        {
            var response = await _service.Idea.Idea(getIdeaDto);
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

        [HttpDelete("{id:int}")]
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
