using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace Data.Repository
{
    public class EventRepo : Repository<Event>, IEventRepo
    {
        private readonly AppDbContext _context;

        public EventRepo(AppDbContext db): base(db)
        {
            _context = db;
        }

             public async Task<bool> CheckPublishedOrActive(int eventId)
             {
                    var res = await _context.Events.Where(e => e.EventId != eventId).AnyAsync(e => e.IsPublished || e.IsCurrentYear);
                    return res;
                }
    }
}
