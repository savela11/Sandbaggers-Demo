using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Interface
{
    public interface IRoleManagerRepo
    {
        Task<IdentityRole> RoleByName(string roleName);
        Task<List<IdentityRole>> Roles();
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> EditRole(IdentityRole role);
        Task<IdentityResult> DeleteRole(IdentityRole role);
        Task<bool> DoesRoleExist(string roleName);
    }
}
