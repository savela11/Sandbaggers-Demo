using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class RoleManagerRepo : IRoleManagerRepo
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerRepo(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole> RoleByName(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<List<IdentityRole>> Roles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> EditRole(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRole(IdentityRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<bool> DoesRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }


    }
}
