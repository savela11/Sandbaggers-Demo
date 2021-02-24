using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface ITeamService
    {
        Task<ServiceResponse<List<TeamVm>>> TeamsByEvent(int eventId);


        Task<ServiceResponse<EventVm>> RemoveTeamFromEvent(RemoveTeamFromEventDto removeTeamFromEventDto);

        Task<ServiceResponse<TeamVm>> CreateTeamForEvent(CreateTeamForEventDto createTeamForEventDto);

        Task<ServiceResponse<List<TeamVm>>> UpdateTeams(List<TeamVm> teamVmList);


    }
}
