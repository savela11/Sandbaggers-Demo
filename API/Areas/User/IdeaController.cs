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
            var serviceResponse = new ServiceResponse<IdeaVm>();

            try
            {
                var idea = await _dbContext.Ideas.FirstOrDefaultAsync(i => i.Id == getIdeaDto.IdeaId);
                if (idea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Idea found by Id";
                    return BadRequest(serviceResponse);
                }

                if (idea.CreatedByUserId != getIdeaDto.UserId)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User Id and Created By User Id do not match";
                    return BadRequest(serviceResponse);
                }

                var foundUser = await _dbContext.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == idea.CreatedByUserId);

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No User Found with Created By User Id";
                    return BadRequest(serviceResponse);
                }

                var ideaVm = new IdeaVm
                {
                    Id = idea.Id,
                    Title = idea.Title,
                    Description = idea.Description,
                    CreatedBy = new CreatedByUserVm
                    {
                        Id = foundUser.Id,
                        FullName = foundUser.FullName,
                        Image = foundUser.UserProfile.Image
                    },
                    CreatedOn = default,
                    UpdatedOn = default
                };


                serviceResponse.Data = ideaVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
                return BadRequest(serviceResponse);
            }

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse.Data);
            }

            return BadRequest(serviceResponse);
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
