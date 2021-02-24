using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Interface
{
    public interface IRoleRepo
    {
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityRole> RoleByName(string roleName);
        Task<IdentityResult> AddUserToRole(ApplicationUser applicationUser, string roleName);
        Task<IdentityResult> RemoveUserFromRole(ApplicationUser applicationUser, string roleName);
        Task<IdentityResult> DeleteRole(IdentityRole role);
        Task<IdentityResult> EditRoleName(IdentityRole foundRole);
        public Task<bool> IsInRole(ApplicationUser user, string roleName);
        Task<List<IdentityRole>> Roles();
        Task<List<ApplicationUser>> UsersForRole();
        Task<ApplicationUser> UserById(string userId);
        Task<bool> DoesRoleExist(string roleName);
        Task<bool> DoesUserExistInRole(ApplicationUser user, string roleName);
    }
}
