using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IContactService
    {
        Task<ServiceResponse<List<ContactVm>>> ContactVmList(List<ApplicationUser> users);
        Task<ServiceResponse<List<ApplicationUser>>> Contacts();
        Task<ServiceResponse<List<ContactVm>>> ContactVmList();
    }
}
