using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IBetService
    {
        Task<ServiceResponse<BetVm>> CreateBet(CreateBetDto createBetDto);
        Task<ServiceResponse<BetVm>> Bet(int betId);
        Task<ServiceResponse<List<BetVm>>> AllActiveBets();
        Task<ServiceResponse<List<BetVm>>> UserBets(string id);
        Task<ServiceResponse<string>> DeleteBet(DeleteBetDto deleteBetDto);
        Task<ServiceResponse<AcceptedByUserVm>> AcceptBet(UserAcceptedBetDto userAcceptedBetDto);
        Task<ServiceResponse<BetVm>> UpdateBet(BetVm betDto);
    }
}
