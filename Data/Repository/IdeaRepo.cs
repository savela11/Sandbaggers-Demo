
using Data.Models;
using Data.Repository.Interface;


namespace Data.Repository
{
    public class IdeaRepo : Repository<Idea>, IIdeaRepo
    {
        private readonly AppDbContext _context;

        public IdeaRepo(AppDbContext db): base(db)
        {
            _context = db;
        }


    }
}
