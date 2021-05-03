using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.ViewModels;
using Models.ViewModels.Views;
using Services.Interface;
using Utilities;

namespace Services
{
    public class DraftService : IDraftService
    {
        private readonly AppDbContext _dbContext;

        public DraftService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<DraftManagerViewData>> AdminDraftManagerData()
        {
            var serviceResponse = new ServiceResponse<DraftManagerViewData>();

            var errorList = new List<string>();

            try
            {
                var foundEvent = await _dbContext.Events.Include(e => e.Teams).Include(e => e.Draft).FirstOrDefaultAsync(e => e.IsCurrentYear);
                // var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.IsCurrentYear, "Teams,Draft");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found By Id";
                    return serviceResponse;
                }


                var draftManagerViewData = new DraftManagerViewData
                {
                    DraftId = foundEvent.Draft.Id,
                    RegisteredUsers = new List<RegisteredUserVm>(),
                    DraftUsers = new List<DraftUserVm>(),
                    DraftCaptains = new List<DraftCaptainVm>()
                };


                // var allUsers = await _unitOfWork.User.GetAll(u => foundEvent.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");
                // var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");
                var allUsers = await _dbContext.Users.Include(u => u.UserProfile).ToListAsync();

                var allUsersList = allUsers.ToList();

                HashSet<string> captainIds = new HashSet<string>(foundEvent.Teams.Select(t => t.CaptainId));
                // HashSet<string> draftCaptIds = new HashSet<string>(foundEvent.Draft.DraftCaptains.Select(c => c.Id));

                var registeredUsers = allUsersList.Where(u => foundEvent.RegisteredUserIds.Contains(u.Id)).ToList();


                HashSet<string> draftUserIds = new HashSet<string>(foundEvent.Draft.DraftUsers.Select(ru => ru.Id));

                draftManagerViewData.DraftUsers = foundEvent.Draft.DraftUsers.Select(u => new DraftUserVm
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    BidAmount = u.BidAmount,
                }).ToList();

                draftManagerViewData.RegisteredUsers = registeredUsers.Where(u => !draftUserIds.Contains(u.Id) && !captainIds.Contains(u.Id)).Select(u => new RegisteredUserVm
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Image = u.UserProfile.Image,
                    Username = u.UserName
                }).ToList();


                // var captains = registeredUsers.Where(u => captainIds.Contains(u.Id) && draftCaptIds.Contains(u.Id)).ToList();
                var draftCaptainVmList = foundEvent.Draft.DraftCaptains.Select(c => new DraftCaptainVm
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    TeamName = foundEvent.Teams.First(t => t.CaptainId == c.Id).Name,
                    Balance = c.Balance
                }).ToList();
                draftManagerViewData.DraftCaptains = draftCaptainVmList;

                if (foundEvent.Teams.Count < 1) errorList.Add("Create Teams for Event");
                if (foundEvent.RegisteredUserIds.Count < 1) errorList.Add("Register Users For Event");
                if (errorList.Count > 0)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Errors = errorList;
                    return serviceResponse;
                }

                serviceResponse.Data = draftManagerViewData;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<DraftManagerViewData>> EditDraft(DraftManagerViewData draftManagerViewData)
        {
            var serviceResponse = new ServiceResponse<DraftManagerViewData>();
            try

            {
                // var draft = await _unitOfWork.Draft.GetFirstOrDefault(d => d.Id == draftManagerViewData.DraftId);
                var draft = await _dbContext.Drafts.FirstOrDefaultAsync(d => d.Id == draftManagerViewData.DraftId);
                if (draft == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Draft found by Id";
                    return serviceResponse;
                }

                draft.DraftUsers = draftManagerViewData.DraftUsers.Select(u => new DraftUser()
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    BidAmount = u.BidAmount,
                }).ToList();
                await _dbContext.SaveChangesAsync();
                serviceResponse.Data = draftManagerViewData;
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
