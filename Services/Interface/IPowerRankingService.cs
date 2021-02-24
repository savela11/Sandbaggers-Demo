using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IPowerRankingService
    {
        Task<ServiceResponse<List<PowerRankingVm>>> PowerRankings();
        Task<ServiceResponse<List<RegisteredUserVm>>> EventRegisteredUsers(int eventId);
        Task<ServiceResponse<PowerRankingVm>> PowerRanking(int eventId);

        Task<ServiceResponse<RankingVm>> CreateUserRanking(CreateRankingDto createRankingDto);
    }
}
