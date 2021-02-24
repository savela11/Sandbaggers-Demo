using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository
{
    public class UserManagerRepo : IUserManagerRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerRepo(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IList<string>> UserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }


        public async Task CreateNewClaim(ApplicationUser user, string claimValue)
        {
            await _userManager.AddClaimAsync(user, new Claim("Claim", claimValue));
        }

        public async Task<IList<Claim>> GetClaims(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }


        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }


        public async Task<IdentityResult> RemoveUserFromRole(ApplicationUser user, string roleName)
        {
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<bool> IsInRole(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
