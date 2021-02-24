using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace Data.Repository
{
    public class BetRepo : Repository<Bet>, IBetRepo
    {
        private readonly AppDbContext _context;

        public BetRepo(AppDbContext db): base(db)
        {
            _context = db;
        }
        //
        // public async Task<Bet> CreateBet(Bet createdBet)
        // {
        //     await _context.Bets.AddAsync(createdBet);
        //     await _context.SaveChangesAsync();
        //     return createdBet;
        // }
        //
        // public async Task<List<Bet>> AllActiveBets()
        // {
        //     return await _context.Bets.OrderByDescending(b => b.CreatedOn).Where(b => b.IsActive).ToListAsync();
        // }
        //
        // public async Task<List<Bet>> UserBets(string id)
        // {
        //     return await _context.Bets.Where(b => b.CreatedByUserId == id).ToListAsync();
        // }
        //
        // public async Task<int> DeleteBet(Bet bet)
        // {
        //     _context.Bets.Remove(bet);
        //
        //     return await _context.SaveChangesAsync();
        // }
        //
        // public async Task<Bet> BetById(int betId)
        // {
        //     return await _context.Bets.FirstOrDefaultAsync(b => b.BetId == betId);
        // }
        //
        // public async Task SaveBet(Bet foundBet)
        // {
        //     _context.Bets.Update(foundBet);
        //     await _context.SaveChangesAsync();
        // }
    }
}
