using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PowerRankingService : IPowerRankingService
    {
        private readonly IUnitOfWork _unitOfWork;


        public PowerRankingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<PowerRankingVm>>> PowerRankings()
        {
            var serviceResponse = new ServiceResponse<List<PowerRankingVm>>();

            try
            {
                var foundEvents = await _unitOfWork.Event.GetAll(orderBy: evnt => evnt.OrderByDescending(e => e.CreatedOn), includeProperties: "PowerRanking");


                var eventPowerRankingsList = new List<PowerRankingVm>();
                foreach (var foundEvent in foundEvents)
                {
                    var powerRankingVm = await PowerRankingMapper.PowerRankingVm(foundEvent);
                    eventPowerRankingsList.Add(powerRankingVm);
                }

                serviceResponse.Data = eventPowerRankingsList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<RegisteredUserVm>>> EventRegisteredUsers(int eventId)
        {
            var serviceResponse = new ServiceResponse<List<RegisteredUserVm>>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(evnt => evnt.EventId == eventId, "PowerRanking");
                var registeredUserVmList = new List<RegisteredUserVm>();
                foreach (var registeredUserId in foundEvent.RegisteredUserIds)
                {
                    var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == registeredUserId, "UserProfile");

                    var registeredUserVm = UserMapper.RegisteredUserVm(foundUser);
                    registeredUserVmList.Add(registeredUserVm);
                }

                serviceResponse.Data = registeredUserVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<PowerRankingVm>> PowerRanking(int eventId)
        {
            var serviceResponse = new ServiceResponse<PowerRankingVm>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(evnt => evnt.EventId == eventId, includeProperties: "PowerRanking");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Power Ranking found by Id";
                    return serviceResponse;
                }

                var powerRankingVm = await PowerRankingMapper.PowerRankingVm(foundEvent);

                serviceResponse.Data = powerRankingVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<RankingVm>> CreateUserRanking(CreateRankingDto createRankingDto)
        {
            var serviceResponse = new ServiceResponse<RankingVm>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(evnt => evnt.EventId == createRankingDto.EventId, includeProperties: "PowerRanking");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No PowerRankings found by Id";
                    return serviceResponse;
                }


                var userRanking = new Ranking
                {
                    RankingId = Guid.NewGuid(),
                    Trending = createRankingDto.Trending,
                    Writeup = createRankingDto.Writeup,
                    CreatedOn = DateTime.Now,
                    UserId = createRankingDto.UserId,
                    Handicap = createRankingDto.Handicap,
                    Rank = createRankingDto.Rank,
                };
                foundEvent.PowerRanking.Rankings.Add(userRanking);

                await _unitOfWork.Save();

                var rankingVm = await PowerRankingMapper.RankingVm(userRanking);


                serviceResponse.Data = rankingVm;
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
