using System.Collections.Generic;
using System.Threading.Tasks;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserVm>>> Users();
        Task<ServiceResponse<UserVm>> FindUserById(string id);

        Task<ServiceResponse<UserVm>> GetUserWithSettings(string id);
    }
}
