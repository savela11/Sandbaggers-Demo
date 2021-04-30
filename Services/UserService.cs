using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;
using Services.Interface;
using Utilities;


namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public UserVm UserVm(ApplicationUser user)
        {
            var userVm = new UserVm
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                Profile = new UserProfileVm
                {
                    Image = user.UserProfile.Image,
                    FirstName = user.UserProfile.FirstName,
                    LastName = user.UserProfile.LastName,
                    Handicap = user.UserProfile.Handicap
                },

                Settings = new UserSettingsVm
                {
                    FavoriteLinks = user.UserSettings.FavoriteLinks.Select(u => new FavoriteLinkVm
                    {
                        Name = u.Name,
                        Link = u.Link
                    }).ToList(),
                    IsContactNumberShowing = user.UserSettings.IsContactNumberShowing,
                    IsContactEmailShowing = user.UserSettings.IsContactEmailShowing
                },
            };
            return userVm;
        }

        public async Task<ServiceResponse<List<UserVm>>> Users()
        {
            var serviceResponse = new ServiceResponse<List<UserVm>>();
            try
            {
                var users = await _unitOfWork.User.GetAll();

                List<UserVm> userVmList = new List<UserVm>();
                foreach (var user in users)
                {
                    var userVm = UserVm(user);


                    userVmList.Add(userVm);
                }

                serviceResponse.Data = userVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserVm>> FindUserById(string id)
        {
            var serviceResponse = new ServiceResponse<UserVm>();
            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == id, includeProperties: "UserProfile,UserSettings");

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User Found";
                }
                else
                {
                    var userVm = UserVm(foundUser);
                    serviceResponse.Data = userVm;
                }
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<UserVm>> GetUserWithSettings(string id)
        {
            var serviceResponse = new ServiceResponse<UserVm>();

            try
            {
                var response = await FindUserById(id);
                if (response.Success == false)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = response.Message;
                }
                else
                {
                    var userWithSettings = response.Data;
                    serviceResponse.Data = userWithSettings;
                }
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
