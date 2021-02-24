
using Data.Models;
using Data.Repository.Interface;


namespace Data.Repository
{
    public class UserRepo : Repository<ApplicationUser>, IUserRepo
    {
        private readonly AppDbContext _context;


        public UserRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }


    }
}
