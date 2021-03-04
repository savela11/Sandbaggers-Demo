using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
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

        public async Task<ServiceResponse<PowerRankingVm>> PowerRankingVm(Event evnt)
        {
            var serviceResponse = new ServiceResponse<PowerRankingVm>();

            try
            {
                var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");
                var allUsersList = allUsers.ToList();
                var powerRankingVm = new PowerRankingVm
                {
                    EventId = evnt.PowerRanking.EventId,
                    EventName = evnt.PowerRanking.Event.Name,
                    Year = evnt.PowerRanking.Year,
                    Disclaimer = evnt.PowerRanking.Disclaimer,
                    Rankings = new List<RankingVm>(),
                    RegisteredUsers = new List<RegisteredUserVm>(),
                    CreatedOn = evnt.PowerRanking.CreatedOn
                };


                foreach (var ranking in evnt.PowerRanking.Rankings)
                {
                    var userRanking = allUsersList.FirstOrDefault(u => u.Id == ranking.UserId);

                    if (userRanking == null) continue;

                    var rankingVm = new RankingVm
                    {
                        RankingId = ranking.RankingId,
                        UserId = userRanking.Id,
                        FullName = userRanking.FullName,
                        Handicap = userRanking.UserProfile.Handicap,
                        Rank = ranking.Rank,
                        Trending = ranking.Trending,
                        Writeup = ranking.Writeup,
                        CreatedOn = ranking.CreatedOn,
                        UpdatedOn = ranking.UpdatedOn
                    };
                    powerRankingVm.Rankings.Add(rankingVm);
                }

                foreach (var userid in evnt.RegisteredUserIds)
                {
                    var registeredUser = allUsersList.FirstOrDefault(u => u.Id == userid);

                    if (registeredUser == null) continue;

                    var registeredUserVm = new RegisteredUserVm
                    {
                        Id = registeredUser.Id,
                        Username = registeredUser.UserName,
                        FullName = registeredUser.FullName,
                        Image = registeredUser.UserProfile.Image
                    };

                    powerRankingVm.RegisteredUsers.Add(registeredUserVm);
                }


                serviceResponse.Data = powerRankingVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
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
                    var powerRankingVmResponse = await PowerRankingVm(foundEvent);

                    eventPowerRankingsList.Add(powerRankingVmResponse.Data);
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

                    var registeredUserVm = new RegisteredUserVm
                    {
                        Id = foundUser.Id,
                        Username = foundUser.UserName,
                        FullName = foundUser.FullName,
                        Image = foundUser.UserProfile.Image
                    };

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

                var powerRankingVmResponse = await PowerRankingVm(foundEvent);
                if (powerRankingVmResponse.Success == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"message";
                    return serviceResponse;
                }


                serviceResponse.Data = powerRankingVmResponse.Data;
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

                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == createRankingDto.UserId);
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No User found by id";
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

                var rankingVm = new RankingVm
                {
                    RankingId = userRanking.RankingId,
                    UserId = foundUser.Id,
                    FullName = foundUser.FullName,
                    Handicap = userRanking.Handicap,
                    Rank = userRanking.Rank,
                    Trending = userRanking.Trending,
                    Writeup = userRanking.Writeup,
                    CreatedOn = userRanking.CreatedOn,
                    UpdatedOn = DateTime.Now
                };


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
