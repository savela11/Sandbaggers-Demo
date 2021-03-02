using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Models.ViewModels;
using Services.Interface;
using Services.Mapper;
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

        public async Task<ServiceResponse<List<ContactVm>>> Contacts()
        {
            var serviceResponse = new ServiceResponse<List<ContactVm>>();

            try
            {
                var allUsers = await _unitOfWork.User.GetAll(orderBy: u => u.OrderBy(i => i.UserProfile.LastName), includeProperties: "UserProfile,UserSettings");


                var contactVmList = ContactMapper.ContactVmList(allUsers.ToList());
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
