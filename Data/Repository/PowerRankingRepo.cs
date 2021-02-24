using Data.Models;
using Data.Repository.Interface;


namespace Data.Repository
{
    public class PowerRankingRepo : Repository<PowerRanking>, IPowerRankingRepo
    {
        private readonly AppDbContext _context;

        public PowerRankingRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }
    }
}
