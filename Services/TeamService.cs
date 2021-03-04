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
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<TeamVm>>> TeamVmList(Event evnt)
        {
            var serviceResponse = new ServiceResponse<List<TeamVm>>();

            try
            {
                var teamVmList = new List<TeamVm>();

                foreach (var team in evnt.Teams)
                {
                    var teamVmResponse = await TeamVm(team);
                    teamVmList.Add(teamVmResponse.Data);
                }

                serviceResponse.Data = teamVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<TeamVm>> TeamVm(Team team)
        {
            var serviceResponse = new ServiceResponse<TeamVm>();

            try
            {
                var teamVm = new TeamVm {Captain = new TeamMemberVm(), Name = team.Name, EventId = team.EventId, TeamId = team.TeamId, Color = team.Color};
                if (string.IsNullOrEmpty(team.Name))
                {
                    team.Name = team.TeamId.ToString();
                }

                if (!string.IsNullOrEmpty(team.CaptainId))
                {
                    var foundCaptain = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == team.CaptainId, includeProperties: "UserProfile");
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
                    foreach (var memberId in team.TeamMemberIds)
                    {
                        var foundTeamMember = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == memberId, includeProperties: "UserProfile");
                        if (foundTeamMember != null)
                        {
                            teamVm.TeamMembers.Add(new TeamMemberVm
                            {
                                Id = foundTeamMember.Id,
                                Image = foundTeamMember.UserProfile.Image,
                                FullName = foundTeamMember.FullName
                            });
                        }
                    }
                }

                serviceResponse.Data = teamVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<TeamVm>>> TeamsByEvent(int eventId)
        {
            var serviceResponse = new ServiceResponse<List<TeamVm>>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == eventId, includeProperties: "Teams");
                // var eventTeams = await _unitOfWork.Team.GetAll(t => t.EventId == eventId);
                // var teamList = eventTeams.ToList();
                if (foundEvent.Teams.Any())
                {
                    var teamVmListResponse = await TeamVmList(foundEvent);

                    serviceResponse.Data = teamVmListResponse.Data;
                }

                else
                {
                    serviceResponse.Data = new List<TeamVm>();
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<TeamVm>>> UpdateTeams(List<TeamVm> teamVmList)
        {
            var serviceResponse = new ServiceResponse<List<TeamVm>>();

            try
            {
                foreach (var teamVm in teamVmList)
                {
                    var foundTeam = await _unitOfWork.Team.GetFirstOrDefault(t => t.TeamId == teamVm.TeamId);

                    if (foundTeam == null) continue;
                    var teamMemberIds = teamVm.TeamMembers.Select(t => t.Id).ToList();
                    foundTeam.CaptainId = teamVm.Captain.Id;
                    foundTeam.Name = teamVm.Name;
                    foundTeam.TeamMemberIds = teamMemberIds;
                    await _unitOfWork.Save();
                }


                serviceResponse.Data = teamVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Event>> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var serviceResponse = new ServiceResponse<Event>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == removeTeamFromEventDto.EventId, "Teams");
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found.";
                    return serviceResponse;
                }

                var foundTeam = foundEvent.Teams.Find(t => t.TeamId == removeTeamFromEventDto.TeamId);

                if (foundTeam == null)
                {
                    serviceResponse.Message = "No Team found to remove";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (foundTeam.TeamMemberIds.Count > 0)
                {
                    serviceResponse.Message = "Must Remove all Team Members before Removing a team.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                foundEvent.Teams.Remove(foundTeam);
                await _unitOfWork.Save();


                serviceResponse.Data = foundEvent;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<TeamVm>> CreateTeamForEvent(CreateTeamForEventDto createTeamForEventDto)
        {
            var serviceResponse = new ServiceResponse<TeamVm>();

            try
            {
                var foundEvent = await _unitOfWork.Event.GetFirstOrDefault(e => e.EventId == createTeamForEventDto.EventId);
                if (foundEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Event Found";
                    return serviceResponse;
                }


                var team = new Team
                {
                    Event = foundEvent,
                    Name = "",
                    CaptainId = "",
                    EventId = foundEvent.EventId,
                    TeamMemberIds = new List<string>(),
                    Color = "Red"
                };


                var createdTeam = await _unitOfWork.Team.AddAsync(team);

                await _unitOfWork.Save();
                var teamVm = new TeamVm
                {
                    TeamId = createdTeam.TeamId,
                    Name = createdTeam.Name,
                    Color = createdTeam.Color,
                    EventId = createdTeam.EventId,
                    Captain = new TeamMemberVm(),
                    TeamMembers = new List<TeamMemberVm>()
                };


                serviceResponse.Data = teamVm;
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
