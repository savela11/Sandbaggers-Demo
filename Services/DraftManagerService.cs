using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Models.ViewModels.Views;
using Services.Interface;
using Utilities;

namespace Services
{
    public class DraftManagerService : IDraftManagerService
    {
        private readonly AppDbContext _dbContext;

        public DraftManagerService(AppDbContext dbContext)
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
                    DraftCaptains = new List<DraftCaptainVm>(),
                    IsDraftLive = foundEvent.Draft.IsDraftLive
                };


                var allUsers = await _dbContext.Users.Include(u => u.UserProfile).ToListAsync();

                var allUsersList = allUsers.ToList();

                HashSet<string> captainIds = new HashSet<string>(foundEvent.Teams.Select(t => t.CaptainId));

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

        public async Task<ServiceResponse<string>> RemoveTeamCaptainFromDraft(AddOrRemoveTeamCaptainDto addOrRemoveTeamCaptainDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundDraft = await _dbContext.Drafts.FirstOrDefaultAsync(d => d.EventId == addOrRemoveTeamCaptainDto.EventId);
                if (foundDraft == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Draft found by Event Id";
                    return serviceResponse;
                }

                var foundCaptain = foundDraft.DraftCaptains.FirstOrDefault(c => c.Id == addOrRemoveTeamCaptainDto.CaptainId);
                if (foundCaptain == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Captain found by Id to remove";
                    return serviceResponse;
                }

                foundDraft.DraftCaptains.Remove(foundCaptain);
                _dbContext.Drafts.Update(foundDraft);
                await _dbContext.SaveChangesAsync();
                serviceResponse.Data = $" {foundCaptain.FullName} has been removed as Captain";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> AddTeamCaptainToDraft(AddOrRemoveTeamCaptainDto addOrRemoveTeamCaptainDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundEvent = await _dbContext.Events.Include(e => e.Teams).Include(e => e.Draft).FirstOrDefaultAsync(d => d.EventId == addOrRemoveTeamCaptainDto.EventId);
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Event found by Event Id";
                    return serviceResponse;
                }

                if (!foundEvent.Teams.Exists(t => t.TeamMemberIds.Contains(addOrRemoveTeamCaptainDto.CaptainId)))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Must be on team before added as Captain";
                    return serviceResponse;
                }


                var foundUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == addOrRemoveTeamCaptainDto.CaptainId);
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }

                var draftCaptain = new DraftCaptain
                {
                    Id = foundUser.Id,
                    FullName = foundUser.FullName,
                    Balance = 0
                };


                foundEvent.Draft.DraftCaptains.Add(draftCaptain);
                _dbContext.Drafts.Update(foundEvent.Draft);
                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = $" {draftCaptain.FullName} has been added as Captain";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateDraftStatus(UpdateDraftStatusDto updateDraftStatusDto)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var foundDraft = await _dbContext.Drafts.FirstOrDefaultAsync(d => d.Id == updateDraftStatusDto.DraftId);
                if (foundDraft == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Draft found by Draft Id";
                    return serviceResponse;
                }

                foundDraft.IsDraftLive = updateDraftStatusDto.Status;
                _dbContext.Drafts.Update(foundDraft);
                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = foundDraft.IsDraftLive;
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
