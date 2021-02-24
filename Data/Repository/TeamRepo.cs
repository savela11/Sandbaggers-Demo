using Data.Models;
using Data.Repository.Interface;


namespace Data.Repository
{
    public class TeamRepo : Repository<Team>, ITeamRepo
    {
        private readonly AppDbContext _context;

        public TeamRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }
    }
}
