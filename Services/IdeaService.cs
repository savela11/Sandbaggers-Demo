using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;


namespace Services
{
    public class IdeaService : IIdeaService
    {
        private readonly AppDbContext _dbContext;

        public IdeaService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ServiceResponse<List<IdeaVm>>> AllIdeas()
        {
            var serviceResponse = new ServiceResponse<List<IdeaVm>>();

            try
            {
                var ideas = await _dbContext.Ideas.ToListAsync();
                var allUsers = await _dbContext.Users.Include(u => u.UserProfile).ToListAsync();

                var ideaVmList = ideas.Select(i => new IdeaVm
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CreatedBy = allUsers.Select(u => new CreatedByUserVm
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Image = u.UserProfile.Image
                    }).FirstOrDefault(u => u.Id == i.CreatedByUserId),
                    CreatedOn = i.CreatedOn.ToString(),
                    UpdatedOn = i.UpdatedOn
                }).ToList();
                serviceResponse.Data = ideaVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IdeaVm>> Idea(GetIdeaDto getIdeaDto)
        {
            var serviceResponse = new ServiceResponse<IdeaVm>();

            try
            {
                var foundIdea = await _dbContext.Ideas.FirstOrDefaultAsync(i => i.Id == getIdeaDto.IdeaId);
                if (foundIdea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Idea found by Id";
                    return serviceResponse;
                }

                var foundUser = await _dbContext.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == getIdeaDto.UserId);

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }


                // Check if user was the one who created the Idea
                if (foundIdea.CreatedByUserId != foundUser.Id)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "User did not create this Idea";
                    return serviceResponse;
                }

                var ideaVm = new IdeaVm
                {
                    Id = foundIdea.Id,
                    Title = foundIdea.Title,
                    Description = foundIdea.Description,
                    CreatedBy = new CreatedByUserVm
                    {
                        Id = foundUser.Id,
                        FullName = foundUser.FullName,
                        Image = foundUser.UserProfile.Image
                    },
                    CreatedOn = foundIdea.CreatedOn.ToString(),
                    UpdatedOn = foundIdea.UpdatedOn
                };

                serviceResponse.Data = ideaVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IdeaVm>> AddIdea(AddIdeaDto addIdeaDto)
        {
            var serviceResponse = new ServiceResponse<IdeaVm>();

            try
            {
                var foundUser = await _dbContext.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == addIdeaDto.UserId);

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found to creat idea";
                    return serviceResponse;
                }

                var idea = new Idea
                {
                    Title = addIdeaDto.Title,
                    Description = addIdeaDto.Description,
                    CreatedByUserId = foundUser.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                var createdIdea = await _dbContext.Ideas.AddAsync(idea);


                await _dbContext.SaveChangesAsync();

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
                    CreatedOn = idea.CreatedOn.ToString(),
                    UpdatedOn = idea.UpdatedOn
                };

                serviceResponse.Data = ideaVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RemoveIdea(int id)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundIdea = await _dbContext.Ideas.FirstOrDefaultAsync(i => i.Id == id);
                if (foundIdea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Idea found by Id";
                    return serviceResponse;
                }

                _dbContext.Ideas.Remove(foundIdea);
                await _dbContext.SaveChangesAsync();
                serviceResponse.Data = "Idea removed.";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IdeaVm>> UpdateIdea(IdeaVm ideaVm)
        {
            var serviceResponse = new ServiceResponse<IdeaVm>();

            try
            {
                var foundIdea = await _dbContext.Ideas.FirstOrDefaultAsync(i => i.Id == ideaVm.Id);
                if (foundIdea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Idea found by Id";
                }
                else
                {
                    foundIdea.Description = ideaVm.Description;
                    foundIdea.Title = ideaVm.Title;
                    foundIdea.UpdatedOn = DateTime.Now;

                    _dbContext.Ideas.Update(foundIdea);
                    await _dbContext.SaveChangesAsync();
                    serviceResponse.Data = ideaVm;
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
