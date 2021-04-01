using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;
using Models.ViewModels.Views;
using Services.Interface;
using Utilities;

namespace Services
{
    public class DraftService : IDraftService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DraftService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<DraftManagerViewData>> AdminDraftManagerData()
        {
            var serviceResponse = new ServiceResponse<DraftManagerViewData>();

            var errorList = new List<string>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.IsCurrentYear, "Teams,Draft");
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

                // MUST HAVE REGISTERED USERS AND REGISTERED TEAM
                if (foundEvent.RegisteredUserIds.Any() && foundEvent.Teams.Any())
                {
                    // var allUsers = await _unitOfWork.User.GetAll(u => foundEvent.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");
                    var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");

                    var allUsersList = allUsers.ToList();


                    var registeredUsers = allUsersList.Where(u => foundEvent.RegisteredUserIds.Contains(u.Id)).ToList();


                    HashSet<string> captainIds = new HashSet<string>(foundEvent.Teams.Select(t => t.CaptainId));
                    HashSet<string> draftCaptIds = new HashSet<string>(foundEvent.Draft.DraftCaptains.Select(c => c.Id));

                    var captains = registeredUsers.Where(u => captainIds.Contains(u.Id) && draftCaptIds.Contains(u.Id)).ToList();


                    // if (foundEvent.Draft.DraftUsers.Any())
                    // {
                    //     HashSet<string> draftUserIds = new HashSet<string>(foundEvent.Draft.DraftUsers.Select(ru => ru.Id));
                    //     draftManagerViewData.RegisteredUsers = registeredUsers.Where(u => !draftUserIds.Contains(u.Id)).Select(u => new RegisteredUserVm
                    //     {
                    //         Id = u.Id,
                    //         FullName = u.FullName,
                    //         Image = u.UserProfile.Image,
                    //         Username = u.UserName
                    //     }).ToList();
                    //
                    //     draftManagerViewData.DraftUsers = foundEvent.Draft.DraftUsers.Select(u => new DraftUserVm
                    //     {
                    //         Id = u.Id,
                    //         FullName = u.FullName,
                    //         BidAmount = u.BidAmount,
                    //     }).ToList();
                    // }
                    // else
                    // {
                    //     draftManagerViewData.RegisteredUsers = registeredUsers.Select(u => new RegisteredUserVm
                    //     {
                    //         Id = u.Id,
                    //         FullName = u.FullName,
                    //         Image = u.UserProfile.Image,
                    //         Username = u.UserName
                    //     }).ToList();
                    // }


                    // foreach (var team in foundEvent.Teams)
                    // {
                    //     var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
                    //     if (string.IsNullOrEmpty(team.Name))
                    //     {
                    //         team.Name = team.TeamId.ToString();
                    //     }
                    //
                    //     if (!string.IsNullOrEmpty(team.CaptainId))
                    //     {
                    //         // var foundCaptain = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
                    //         ApplicationUser foundCaptain = allUsersList.FirstOrDefault(u => u.Id == team.CaptainId);
                    //         if (foundCaptain != null)
                    //         {
                    //             teamVm.Captain.Id = foundCaptain.Id;
                    //             teamVm.Captain.Image = foundCaptain.UserProfile.Image;
                    //             teamVm.Captain.FullName = foundCaptain.FullName;
                    //         }
                    //     }
                    //
                    //     teamVm.TeamMembers = new List<TeamMemberVm>();
                    //     if (team.TeamMemberIds.Count > 0)
                    //     {
                    //         var teamMembers = allUsersList.Where(u => team.TeamMemberIds.Contains(u.Id)).Select(u => new TeamMemberVm
                    //         {
                    //             Id = u.Id,
                    //             Image = u.UserProfile.Image,
                    //             FullName = u.FullName
                    //         }).ToList();
                    //
                    //
                    //         teamVm.TeamMembers = teamMembers;
                    //     }
                    //
                    //     draftManagerViewData.Teams.Add(teamVm);
                    // }
                }

                if (foundEvent.Teams.Count < 1) errorList.Add("Create Teams for Event");
                if(foundEvent.RegisteredUserIds.Count < 1) errorList.Add("Register Users For Event");
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
                var draft = await _unitOfWork.Draft.GetFirstOrDefault(d => d.Id == draftManagerViewData.DraftId);
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
                await _unitOfWork.Save();
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
