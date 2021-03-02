using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
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

        public async Task<ServiceResponse<List<UserVm>>> Users()
        {
            var serviceResponse = new ServiceResponse<List<UserVm>>();
            try
            {
                var users = await _unitOfWork.User.GetAll();

                List<UserVm> userVmList = new List<UserVm>();
                foreach (var user in users)
                {
                    var userVm = UserMapper.UserVm(user);

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
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == id, includeProperties:"UserProfile,UserSettings");

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User Found";
                }
                else
                {
                    var userVm = UserMapper.UserVm(foundUser);
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
