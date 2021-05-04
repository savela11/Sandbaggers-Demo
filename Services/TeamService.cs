using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _dbContext;

        public TeamService(IUnitOfWork unitOfWork, AppDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
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


    }
}
