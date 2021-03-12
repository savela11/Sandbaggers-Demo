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
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<List<ApplicationUser>>> Contacts()
        {
            var serviceResponse = new ServiceResponse<List<ApplicationUser>>();

            try
            {
                var allUsers = await _unitOfWork.User.GetAll(orderBy: u => u.OrderBy(i => i.UserProfile.LastName), includeProperties: "UserProfile,UserSettings");
                serviceResponse.Data = allUsers.ToList();
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ContactVm>>> ContactVmList()
        {
            var serviceResponse = new ServiceResponse<List<ContactVm>>();

            try
            {
                var allUsers = await _unitOfWork.User.GetAll(includeProperties: "UserProfile,UserSettings");
                var contactVmList = allUsers.Select(u => new ContactVm
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Image = u.UserProfile.Image,
                    IsContactNumberShowing = u.UserSettings.IsContactNumberShowing,
                    IsContactEmailShowing = u.UserSettings.IsContactEmailShowing
                }).ToList();
                serviceResponse.Data = contactVmList;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ContactVm>> ContactVm(ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<ContactVm>();

            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == user.Id);
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User Found";
                    return serviceResponse;
                }

                var contactVm = new ContactVm
                {
                    Id = foundUser.Id,
                    FullName = foundUser.FullName,
                    PhoneNumber = foundUser.PhoneNumber,
                    Email = foundUser.Email,
                    Image = foundUser.UserProfile.Image,
                    IsContactNumberShowing = foundUser.UserSettings.IsContactNumberShowing,
                    IsContactEmailShowing = foundUser.UserSettings.IsContactEmailShowing
                };

                serviceResponse.Data = contactVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ContactVm>>> ContactVmList(List<ApplicationUser> users)
        {
            var serviceResponse = new ServiceResponse<List<ContactVm>>();

            try
            {
                var contactVmList = new List<ContactVm>();
                foreach (var user in users)
                {
                    var contactVmResponse = await ContactVm(user);

                    if (contactVmResponse.Success)
                    {
                        contactVmList.Add(contactVmResponse.Data);
                    }
                }

                serviceResponse.Data = contactVmList;
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
