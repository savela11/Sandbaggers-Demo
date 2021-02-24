using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models.ViewModels;
using Models.ViewModels.Views;
using Services.Interface;
using Services.Mapper;
using Utilities;

namespace Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResponse<DashboardViewData>> DashboardData()
        {
            var serviceResponse = new ServiceResponse<DashboardViewData>();
            try
            {
                var eventResults = await _unitOfWork.EventResults.GetFirstOrDefault(r => r.IsActive);
                var users = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");
                var scrambleChamps = new List<ScrambleChampVm>();
                var applicationUsers = users.ToList();
                if (eventResults != null && eventResults.ScrambleChamps.Count > 0)
                {
                    scrambleChamps = applicationUsers.Where(u => eventResults.ScrambleChamps.Contains(u.Id))
                        .Select(u => new ScrambleChampVm {Id = u.Id, Image = u.UserProfile.Image, FullName = u.FullName}).ToList();
                }

                var sandbaggerWithHandicapVms = applicationUsers.Select(s => new SandbaggerWithHandicapVm
                {
                    Id = s.Id,
                    Image = s.UserProfile.Image,
                    Handicap = s.UserProfile.Handicap,
                    FullName = $"{s.UserProfile.FirstName} {s.UserProfile.LastName}"
                }).ToList();


                var dashboardData = new DashboardViewData
                {
                    ScrambleChamps = scrambleChamps,
                    SandbaggersWithHandicaps = sandbaggerWithHandicapVms
                };

                serviceResponse.Data = dashboardData;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BetVm>>> ActiveBets()
        {
            var serviceResponse = new ServiceResponse<List<BetVm>>();

            try
            {
                var activeBets = await _unitOfWork.Bet.GetAll(b => b.IsActive, orderBy: b => b.OrderByDescending(o => o.CreatedOn));

                var betVmList = await BetMapper.BetVmList(activeBets);

                serviceResponse.Data = betVmList;
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
