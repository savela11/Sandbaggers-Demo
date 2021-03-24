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

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.IsCurrentYear, "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found By Id";
                    return serviceResponse;
                }


                var draftManagerViewData = new DraftManagerViewData
                {
                    RegisteredUsers = new List<RegisteredUserVm>(),
                    Teams = new List<TeamVm>()
                };

                if (foundEvent.RegisteredUserIds.Any())
                {
                    // var allUsers = await _unitOfWork.User.GetAll(u => foundEvent.RegisteredUserIds.Contains(u.Id), includeProperties: "UserProfile");
                    var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");

                    var allUsersList = allUsers.ToList();

                    var registeredUsers = allUsersList.Where(u => foundEvent.RegisteredUserIds.Contains(u.Id)).Select(u => new RegisteredUserVm
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        Image = u.UserProfile.Image,
                        Username = u.UserName
                    }).ToList();

                    if (foundEvent.Teams.Any())
                    {
                        foreach (var team in foundEvent.Teams)
                        {
                            var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
                            if (string.IsNullOrEmpty(team.Name))
                            {
                                team.Name = team.TeamId.ToString();
                            }

                            if (!string.IsNullOrEmpty(team.CaptainId))
                            {
                                // var foundCaptain = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
                                ApplicationUser foundCaptain = allUsersList.FirstOrDefault(u => u.Id == team.CaptainId);
                                if (foundCaptain != null)
                                {
                                    teamVm.Captain.Id = foundCaptain.Id;
                                    teamVm.Captain.Image = foundCaptain.UserProfile.Image;
                                    teamVm.Captain.FullName = foundCaptain.FullName;
                                }
                            }

                            teamVm.TeamMembers = new List<TeamMemberVm>();
                            if (team.TeamMemberIds.Count > 0)
                            {
                                var teamMembers = allUsersList.Where(u => team.TeamMemberIds.Contains(u.Id)).Select(u => new TeamMemberVm
                                {
                                    Id = u.Id,
                                    Image = u.UserProfile.Image,
                                    FullName = u.FullName
                                }).ToList();

                                // foreach (var memberId in team.TeamMemberIds)
                                // {
                                //     var foundTeamMember = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == memberId, includeProperties: "UserProfile");
                                //     if (foundTeamMember != null)
                                //     {
                                //         teamVm.TeamMembers.Add(new TeamMemberVm
                                //         {
                                //             Id = foundTeamMember.Id,
                                //             Image = foundTeamMember.UserProfile.Image,
                                //             FullName = foundTeamMember.FullName
                                //         });
                                //     }
                                // }
                                teamVm.TeamMembers = teamMembers;
                            }

                            draftManagerViewData.Teams.Add(teamVm);
                        }
                    }

                    draftManagerViewData.RegisteredUsers = registeredUsers;
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
    }
}
