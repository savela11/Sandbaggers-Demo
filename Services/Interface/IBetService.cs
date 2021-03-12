using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IBetService
    {
        Task<ServiceResponse<BetVm>> BetVm(Bet bet);
        Task<ServiceResponse<List<BetVm>>> BetVmList(List<Bet> bets);
        Task<ServiceResponse<Bet>> CreateBet(CreateBetDto createBetDto);
        Task<ServiceResponse<Bet>> BetById(int betId);
        Task<ServiceResponse<List<Bet>>> AllActiveBets();
        Task<ServiceResponse<List<Bet>>> UserBets(string id);
        Task<ServiceResponse<string>> DeleteBet(DeleteBetDto deleteBetDto);
        Task<ServiceResponse<AcceptedByUserVm>> AcceptBet(UserAcceptedBetDto userAcceptedBetDto);
        Task<ServiceResponse<Bet>> UpdateBet(BetVm betDto);
        Task<ServiceResponse<List<BetVm>>> MyBets(string userId);
    }
}
