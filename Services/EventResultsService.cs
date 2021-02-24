using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
using Utilities;


namespace Services
{
    public class EventResultsService : IEventResultsService
    {
        private readonly IUnitOfWork _unitOfWork;
   private readonly EventMapper _eventMapper;
        public EventResultsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _eventMapper = new EventMapper(unitOfWork);
        }

        public async Task<ServiceResponse<List<ScrambleChampVm>>> ScrambleChamps()
        {
            var serviceResponse = new ServiceResponse<List<ScrambleChampVm>>();
            try
            {
                var eventResults = await _unitOfWork.EventResults.GetFirstOrDefault(e => e.IsActive);
                var eventResultVm = await _eventMapper.EventResultsVm(eventResults);

                if (eventResults == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Scramble Champs set";
                }
                else
                {
                    serviceResponse.Data = eventResultVm.ScrambleChamps;
                }
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
                }
                else
                {
                    var eventResultVm = await _eventMapper.EventResultsVm(foundEventResults);
                    serviceResponse.Data = eventResultVm;
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
