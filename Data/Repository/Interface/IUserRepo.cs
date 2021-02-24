using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Repository.Interface
{
    public interface IUserRepo : IRepository<ApplicationUser>
    {
        // Task<ApplicationUser> UserById(string id);
        // Task<List<ApplicationUser>> Users();
        //
        // Task<ApplicationUser> UserByUsername(string userName);
        //
        //
        //
        //
        //
        // Task<bool> UserExists(string userId);

    }
}
