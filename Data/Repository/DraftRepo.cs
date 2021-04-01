using Data.Models;
using Data.Repository.Interface;

namespace Data.Repository
{
    public class DraftRepo : Repository<Draft>, IDraftRepo
    {
        private readonly AppDbContext _context;

        public DraftRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }
    }

}
