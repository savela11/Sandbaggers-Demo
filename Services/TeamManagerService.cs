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
    public class TeamManagerService : ITeamManagerService
    {
        private readonly AppDbContext _dbContext;

        public TeamManagerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<List<TeamVm>>> UpdateTeams(List<TeamVm> teamVmList)
        {
            var serviceResponse = new ServiceResponse<List<TeamVm>>();

            try
            {
                var allTeams = await _dbContext.Teams.ToListAsync();
                var teamList = allTeams.ToList();
                foreach (var teamVm in teamVmList)
                {
                    var foundTeam = teamList.FirstOrDefault(t => t.TeamId == teamVm.TeamId);

                    if (foundTeam == null) continue;
                    var teamMemberIds = teamVm.TeamMembers.Select(t => t.Id).ToList();

                    if (!teamMemberIds.Contains(foundTeam.CaptainId) && foundTeam.TeamMemberIds.Contains(foundTeam.CaptainId))
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Remove Team Member from Captain first before removing from Team";
                        return serviceResponse;
                    }

                    foundTeam.Name = teamVm.Name;
                    foundTeam.TeamMemberIds = teamMemberIds;
                    foundTeam.Color = teamVm.Color;
                    await _dbContext.SaveChangesAsync();
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

        public async Task<ServiceResponse<string>> AddTeamCaptain(AddOrRemoveTeamCaptainDto addTeamCaptainDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundTeam = await _dbContext.Teams.FirstOrDefaultAsync(t => t.TeamId == addTeamCaptainDto.TeamId);
                if (foundTeam == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Team found by TeamId";
                    return serviceResponse;
                }

                //MUST BE A TEAM MEMBER TO BECOME CAPTAIN
                if (!foundTeam.TeamMemberIds.Contains(addTeamCaptainDto.CaptainId))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Cannot Add Captain unless already a team member";
                    return serviceResponse;
                }

                foundTeam.CaptainId = addTeamCaptainDto.CaptainId;


                _dbContext.Teams.Update(foundTeam);

                await _dbContext.SaveChangesAsync();


                serviceResponse.Data = $"Team Captain Added";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RemoveTeamCaptain(AddOrRemoveTeamCaptainDto removeTeamCaptainDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundTeam = await _dbContext.Teams.FirstOrDefaultAsync(t => t.EventId == removeTeamCaptainDto.EventId && t.TeamId == removeTeamCaptainDto.TeamId);
                if (foundTeam == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Team found with Team Id";
                    return serviceResponse;
                }



                foundTeam.CaptainId = null;

                _dbContext.Teams.Update(foundTeam);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = "Team Captain Removed";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundTeam = await _dbContext.Teams.FirstOrDefaultAsync(t => t.EventId == removeTeamFromEventDto.EventId);

                if (foundTeam == null)
                {
                    serviceResponse.Message = "No Team found to remove";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (foundTeam.TeamMemberIds.Count > 0 && foundTeam.TeamMemberIds[0] != null)
                {
                    serviceResponse.Message = "Must Remove all Team Members before Removing a team.";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                _dbContext.Remove(foundTeam);
                await _dbContext.SaveChangesAsync();


                serviceResponse.Data = $"{foundTeam.Name} has been removed";
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
                var foundEvent = await _dbContext.Events.FirstOrDefaultAsync(e => e.EventId == createTeamForEventDto.EventId);
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

                var createdTeam = await _dbContext.Teams.AddAsync(team);

                await _dbContext.SaveChangesAsync();
                var teamVm = new TeamVm
                {
                    TeamId = createdTeam.Entity.TeamId,
                    Name = createdTeam.Entity.Name,
                    Color = createdTeam.Entity.Color,
                    EventId = createdTeam.Entity.EventId,
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
