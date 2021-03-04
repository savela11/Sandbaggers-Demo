using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models.ViewModels;
using Services.Interface;
using Utilities;


namespace Services
{
    public class EventResultsService : IEventResultsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventResultsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<ScrambleChampVm>>> ScrambleChamps()
        {
            var serviceResponse = new ServiceResponse<List<ScrambleChampVm>>();
            try
            {
                var eventResults = await _unitOfWork.EventResults.GetFirstOrDefault(e => e.IsActive);
                if (eventResults == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Results Found";
                    return serviceResponse;
                }


                var users = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");
                var scrambleChampVms = new List<ScrambleChampVm>();
                var applicationUsers = users.ToList();
                if (eventResults.ScrambleChamps.Count > 0)
                {
                    scrambleChampVms = applicationUsers.Where(u => eventResults.ScrambleChamps.Contains(u.Id))
                        .Select(u => new ScrambleChampVm {Id = u.Id, Image = u.UserProfile.Image, FullName = u.FullName}).ToList();
                }

                serviceResponse.Data = scrambleChampVms;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<EventResultVm>> EventResults(int id)
        {
            var serviceResponse = new ServiceResponse<EventResultVm>();

            try
            {
                var foundEventResults = await _unitOfWork.EventResults.GetFirstOrDefault(r => r.EventId == id);
                if (foundEventResults == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event found with current ID";
                    return serviceResponse;
                }

                var eventResultsVm = new EventResultVm
                {
                    EventId = foundEventResults.EventId,
                    Teams = null,
                    ScrambleChamps = null,
                    IsActive = false
                };

                serviceResponse.Data = eventResultsVm;
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
