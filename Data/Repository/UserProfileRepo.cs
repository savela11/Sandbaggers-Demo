using Data.Models;
using Data.Repository.Interface;

namespace Data.Repository
{
    public class UserProfileRepo : Repository<UserProfile>, IUserProfileRepo
    {
        private readonly AppDbContext _context;

        public UserProfileRepo(AppDbContext db) : base(db)
        {
            _context = db;
        }
    }
}
