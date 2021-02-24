using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Data.Repository
{
    public class RoleRepo : IRoleRepo
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public RoleRepo(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        public async Task<List<IdentityRole>> Roles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<List<ApplicationUser>> UsersForRole()
        {
            return await _userManager.Users.Include(u => u.UserProfile).ToListAsync();
        }

        public async Task<ApplicationUser> UserById(string userId)
        {
            return await _userManager.Users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IdentityRole> RoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role;
        }


        public async Task<IdentityResult> CreateRole(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> AddUserToRole(ApplicationUser applicationUser, string roleName)
        {
            return await _userManager.AddToRoleAsync(applicationUser, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRole(ApplicationUser applicationUser, string roleName)
        {
            return await _userManager.RemoveFromRoleAsync(applicationUser, roleName);
        }

        public async Task<IdentityResult> DeleteRole(IdentityRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }


        public async Task<IdentityResult> EditRoleName(IdentityRole foundRole)
        {
            var result = await _roleManager.UpdateAsync(foundRole);
            return result;
        }

        public async Task<bool> IsInRole(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> DoesRoleExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<bool> DoesUserExistInRole(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}
