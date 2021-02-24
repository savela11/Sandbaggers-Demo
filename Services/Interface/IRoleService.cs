using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;


namespace Services.Interface
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<RoleVm>>> Roles();
        Task<ServiceResponse<UserWithRoleVm>> AddUserToRole(AddOrRemoveUserFromRoleDto addUserToRoleDto);
        Task<ServiceResponse<UserWithRoleVm>> RemoveUserFromRole(AddOrRemoveUserFromRoleDto removeUserFromRoleDto);
        Task<ServiceResponse<string>> CreateRole(string roleName);
    }
}
