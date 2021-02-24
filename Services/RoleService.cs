using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResponse<List<RoleVm>>> Roles()
        {
            var serviceResponse = new ServiceResponse<List<RoleVm>>();

            try
            {
                var roles = await _unitOfWork.RoleManager.Roles();
                var usersForRole = await _unitOfWork.User.GetAll(includeProperties: "UserProfile");

                List<RoleVm> listOfRoles = new List<RoleVm>();
                var applicationUsers = usersForRole.ToList();
                foreach (var role in roles)
                {
                    if (role.Name == "User") continue;
                    var roleVm = new RoleVm
                    {
                        RoleName = role.Name,
                        Members = new List<UserWithRoleVm>(),
                        NonMembers = new List<UserWithRoleVm>()
                    };
                    foreach (var user in applicationUsers)
                    {
                        var isInRole = await _unitOfWork.UserManager.IsInRole(user, role.Name);
                        var userWithRoleVm = new UserWithRoleVm
                        {
                            Id = user.Id,
                            FullName = $"{user.UserProfile.FirstName} {user.UserProfile.LastName}"
                        };
                        if (isInRole)
                        {
                            roleVm.Members.Add(userWithRoleVm);
                        }
                        else
                        {
                            roleVm.NonMembers.Add(userWithRoleVm);
                        }
                    }

                    listOfRoles.Add(roleVm);
                }

                serviceResponse.Data = listOfRoles;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserWithRoleVm>> AddUserToRole(AddOrRemoveUserFromRoleDto addUserToRoleDto)
        {
            var serviceResponse = new ServiceResponse<UserWithRoleVm>();

            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == addUserToRoleDto.UserId, "UserProfile");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }


                var doesRoleExist = await _unitOfWork.RoleManager.DoesRoleExist(addUserToRoleDto.RoleName);
                if (doesRoleExist == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Role found with the name {addUserToRoleDto.RoleName}";
                    return serviceResponse;
                }

                var doesUserExistInRole = await _unitOfWork.UserManager.IsInRole(foundUser, addUserToRoleDto.RoleName);
                if (doesUserExistInRole)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User already exists in {addUserToRoleDto.RoleName} role";
                    return serviceResponse;
                }

                await _unitOfWork.UserManager.AddUserToRole(foundUser, addUserToRoleDto.RoleName);


                var userWithRoleVm = new UserWithRoleVm
                {
                    Id = foundUser.Id,
                    FullName = foundUser.FullName
                };


                serviceResponse.Data = userWithRoleVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserWithRoleVm>> RemoveUserFromRole(AddOrRemoveUserFromRoleDto removeUserFromRoleDto)
        {
            var serviceResponse = new ServiceResponse<UserWithRoleVm>();

            try
            {
                if (removeUserFromRoleDto.RoleName == "User")
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Cannot remove User's from the User role";
                    return serviceResponse;
                }

                var foundUser = await _unitOfWork.User.GetFirstOrDefault(user => user.Id == removeUserFromRoleDto.UserId, "UserProfile");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }


                var doesRoleExist = await _unitOfWork.RoleManager.DoesRoleExist(removeUserFromRoleDto.RoleName);
                if (doesRoleExist == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"No Role found with the name {removeUserFromRoleDto.RoleName}";
                    return serviceResponse;
                }

                var doesUserExistInRole = await _unitOfWork.UserManager.IsInRole(foundUser, removeUserFromRoleDto.RoleName);
                if (doesUserExistInRole == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"User does not exists in {removeUserFromRoleDto.RoleName} role";
                    return serviceResponse;
                }

                await _unitOfWork.UserManager.RemoveUserFromRole(foundUser, removeUserFromRoleDto.RoleName);
                var userWithRoleVm = new UserWithRoleVm
                {
                    Id = foundUser.Id,
                    FullName = foundUser.FullName
                };

                serviceResponse.Data = userWithRoleVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> CreateRole(string roleName)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var doesRoleExist = await _unitOfWork.RoleManager.DoesRoleExist(roleName);
                if (doesRoleExist)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{roleName} already exists";
                    return serviceResponse;
                }

                var createdRole = await _unitOfWork.RoleManager.CreateRole(roleName);
                if (createdRole.Succeeded == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Error creating role";
                    return serviceResponse;
                }

                serviceResponse.Data = $"{roleName} role has been created";
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
