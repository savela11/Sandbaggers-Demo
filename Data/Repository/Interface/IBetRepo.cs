using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;


namespace Data.Repository.Interface
{
    public interface IBetRepo : IRepository<Bet>
    {
        Task<Bet> CreateBet(Bet createdBet);
        Task<List<Bet>> AllActiveBets();
        // Task<List<Bet>> UserBets(string id);
        // Task<int> DeleteBet(Bet bet);
        // Task<Bet> BetById(int betId);
        // Task SaveBet(Bet foundBet);
    }
}
