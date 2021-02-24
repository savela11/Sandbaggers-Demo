using System.Threading.Tasks;
using Data.Models;

namespace Data.Repository.Interface
{
    public interface IEventRepo : IRepository<Event>
    {
      Task<bool> CheckPublishedOrActive(int eventId);

    }
}
