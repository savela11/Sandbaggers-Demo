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
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EventMapper _eventMapper;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _eventMapper = new EventMapper(unitOfWork);
        }

        public async Task<ServiceResponse<List<TeamVm>>> TeamsByEvent(int eventId)
        {
            var serviceResponse = new ServiceResponse<List<TeamVm>>();

            try
            {
                var eventTeams = await _unitOfWork.Team.GetAll(t => t.EventId == eventId);
                var teamList = eventTeams.ToList();
                if (teamList.Any())
                {
                    var teamListVm = await TeamMapper.TeamVmList(teamList);
                    serviceResponse.Data = teamListVm;
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
                var teamList = new List<Team>();
                foreach (var teamVm in teamVmList)
                {
                    var foundTeam = await _unitOfWork.Team.GetFirstOrDefault(t => t.TeamId == teamVm.TeamId);

                    if (foundTeam == null) continue;
                    var teamMemberIds = teamVm.TeamMembers.Select(t => t.Id).ToList();
                    foundTeam.CaptainId = teamVm.Captain.Id;
                    foundTeam.Name = teamVm.Name;
                    foundTeam.TeamMemberIds = teamMemberIds;
                    await _unitOfWork.Save();
                    teamList.Add(foundTeam);
                }


                var createdTeamVmList = await TeamMapper.TeamVmList(teamList);
                serviceResponse.Data = createdTeamVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<EventVm>> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto)
        {
            var serviceResponse = new ServiceResponse<EventVm>();

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


                // var eventVm = new EventVm
                // {
                //     EventId = foundEvent.EventId,
                //     Name = foundEvent.Name,
                //     Location = foundEvent.Location,
                //     Itineraries = foundEvent.Itineraries,
                //     RegisteredUsers = foundEvent.RegisteredUserIds,
                //     Teams = foundEvent.Teams,
                //     Year = foundEvent.Year,
                //     IsCurrentYear = foundEvent.IsCurrentYear,
                //     IsPublished = foundEvent.IsPublished,
                //     CreatedOn = foundEvent.CreatedOn,
                //     UpdatedOn = foundEvent.UpdatedOn
                // };

                var eventVm = await _eventMapper.EventVm(foundEvent);


                serviceResponse.Data = eventVm;
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

                var teamVm = await TeamMapper.TeamVm(createdTeam);

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
