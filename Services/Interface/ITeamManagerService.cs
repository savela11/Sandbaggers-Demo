using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface ITeamManagerService
    {
        Task<ServiceResponse<string>> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto);

        Task<ServiceResponse<TeamVm>> CreateTeamForEvent(CreateTeamForEventDto createTeamForEventDto);

        Task<ServiceResponse<List<TeamVm>>> UpdateTeams(List<TeamVm> teamVmList);
        Task<ServiceResponse<string>> RemoveTeamCaptain(AddOrRemoveTeamCaptainDto removeTeamCaptainDto);
        Task<ServiceResponse<string>> AddTeamCaptain(AddOrRemoveTeamCaptainDto addTeamCaptainDto);
    }
}
