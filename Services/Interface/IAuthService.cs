using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTO;
using Models.ViewModels;
using Utilities;

namespace Services.Interface
{
    public interface IAuthService
    {
        Task<ServiceResponse<List<string>>> RegisterMultipleUsers(
            List<RegisterUserDto> registerUserDtoList);
        Task<ServiceResponse<ApplicationUserVm>> RegisterUser(RegisterUserDto registerUserDto);
        Task<ServiceResponse<LoggedInUserVm>> LoginUser(LoginUserDto loginUserDto);
        Task<ServiceResponse<string>> LogoutUser();
    }
}
