using System.Threading.Tasks;
using Models.ViewModels;
using Models.ViewModels.Views;
using SandbaggersAPI.Services;
using Utilities;


namespace Services.Interface
{
    public interface IProfileService
    {
        Task<ServiceResponse<UserVm>> UpdateUser(UserVm userVm);
        Task<ServiceResponse<UserProfileViewData>> UserProfile(string userId);
        Task<ServiceResponse<BetVm>> UpdateBet(BetVm betVm);
        Task<ServiceResponse<string>> DeleteBet(BetVm betVm);
        Task<ServiceResponse<string>> UpdateProfileImage(string userId, string profileImage);
    }
}
