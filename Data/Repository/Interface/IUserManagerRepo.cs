using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Interface
{
    public interface IUserManagerRepo
    {
        Task<IdentityResult> CreateUser(ApplicationUser user, string password);

        Task<IList<string>> UserRoles(ApplicationUser user);

        Task CreateNewClaim(ApplicationUser user, string claimValue);
        Task<IList<Claim>> GetClaims(ApplicationUser user);

        Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName);

        Task<IdentityResult> RemoveUserFromRole(ApplicationUser user, string roleName);

        Task<bool> IsInRole(ApplicationUser user, string roleName);
    }
}
