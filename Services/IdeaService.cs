using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
using Utilities;


namespace Services
{
    public class IdeaService : IIdeaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public IdeaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<IdeaVm>>> AllIdeas()
        {
            var serviceResponse = new ServiceResponse<List<IdeaVm>>();

            try
            {
                var ideas = await _unitOfWork.Idea.GetAll();
                var ideaVmList = await IdeaMapper.IdeaVmList(ideas);

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
                var foundIdea = await _unitOfWork.Idea.GetFirstOrDefault(i => i.Id == getIdeaDto.IdeaId);
                if (foundIdea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Idea found by Id";
                    return serviceResponse;
                }

                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == getIdeaDto.UserId);
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

                var ideaVm = await IdeaMapper.IdeaVm(foundIdea);
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
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == addIdeaDto.UserId, "UserProfile");

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found to creat idea";
                }
                else
                {
                    var idea = new Idea
                    {
                        Title = addIdeaDto.Title,
                        Description = addIdeaDto.Description,
                        CreatedByUserId = foundUser.Id,
                        CreatedOn = DateTime.UtcNow,
                        UpdatedOn = DateTime.UtcNow
                    };

                    var createdIdea = await _unitOfWork.Idea.AddAsync(idea);
                    await _unitOfWork.Save();

                    serviceResponse.Data = await IdeaMapper.IdeaVm(createdIdea);
                }
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
                var foundIdea = await _unitOfWork.Idea.GetFirstOrDefault(i => i.Id == id);
                if (foundIdea == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Idea found by Id";
                    return serviceResponse;
                }

                await _unitOfWork.Idea.RemoveAsync(foundIdea.Id);
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
                var foundIdea = await _unitOfWork.Idea.GetFirstOrDefault(i => i.Id == ideaVm.Id);
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

                    await _unitOfWork.Save();
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
