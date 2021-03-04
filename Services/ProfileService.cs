using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResponse<UserVm>> UpdateUser(UserVm userVm)
        {
            var serviceResponse = new ServiceResponse<UserVm>();
            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == userVm.Id, includeProperties: "UserProfile,UserSettings");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }

                foundUser.PhoneNumber = userVm.PhoneNumber;
                foundUser.Email = userVm.Email;
                foundUser.FullName = userVm.Profile.FirstName + " " + userVm.Profile.LastName;
                // Profile
                foundUser.UserProfile.Handicap = userVm.Profile.Handicap;
                foundUser.UserProfile.FirstName = userVm.Profile.FirstName;
                foundUser.UserProfile.LastName = userVm.Profile.LastName;
                foundUser.UserProfile.Image = userVm.Profile.Image;
                foundUser.UserProfile.UpdatedOn = DateTime.UtcNow;


                // Settings
                foundUser.UserSettings.FavoriteLinks = userVm.Settings.FavoriteLinks.Select(s => new FavoriteLink
                {
                    Name = s.Name,
                    Link = s.Link
                }).ToList();
                foundUser.UserSettings.IsContactEmailShowing = userVm.Settings.IsContactEmailShowing;
                foundUser.UserSettings.IsContactNumberShowing = userVm.Settings.IsContactNumberShowing;


                await _unitOfWork.Save();


                // var updatedUserVm = new UserVm
                // {
                //     Id = foundUser.Id,
                //     Username = foundUser.UserName,
                //     Email = foundUser.Email,
                //     PhoneNumber = foundUser.PhoneNumber,
                //     FullName = foundUser.FullName,
                //     Profile = new UserProfileVm
                //     {
                //         Image = foundUser.UserProfile.Image,
                //         FirstName = foundUser.UserProfile.FirstName,
                //         LastName = foundUser.UserProfile.LastName,
                //         Handicap = foundUser.UserProfile.Handicap
                //     },
                //     Settings = new UserSettingsVm
                //     {
                //         FavoriteLinks = foundUser.UserSettings.FavoriteLinks.Select(f => new FavoriteLinkVm
                //         {
                //             Name = f.Name,
                //             Link = f.Link
                //         }).ToList(),
                //         IsContactNumberShowing = foundUser.UserSettings.IsContactNumberShowing,
                //         IsContactEmailShowing = foundUser.UserSettings.IsContactEmailShowing
                //     },
                // };


                serviceResponse.Data = userVm;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        // public async Task<ServiceResponse<UserProfileViewData>> UserProfile(string userId)
        // {
        //     var serviceResponse = new ServiceResponse<UserProfileViewData>();
        //
        //     try
        //     {
        //         var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == userId, includeProperties: "UserProfile,UserSettings");
        //
        //         if (foundUser == null)
        //         {
        //             serviceResponse.Success = false;
        //             serviceResponse.Message = "No User Found by id";
        //             return serviceResponse;
        //         }
        //
        //
        //         var userBets = await _unitOfWork.Bet.GetAll(b => b.CreatedByUserId == userId);
        //
        //         serviceResponse.Data = new UserProfileViewData
        //         {
        //             UserVm = UserMapper.UserVm(foundUser),
        //             Bets = await BetMapper.BetVmList(userBets)
        //         };
        //     }
        //     catch (Exception e)
        //     {
        //         serviceResponse.Message = e.Message;
        //         serviceResponse.Success = false;
        //     }
        //
        //     return serviceResponse;
        // }

        public async Task<ServiceResponse<string>> UpdateProfileImage(string userId, string profileImage)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == userId, includeProperties: "UserProfile");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User found by Id";
                    return serviceResponse;
                }

                foundUser.UserProfile.Image = profileImage;
                await _unitOfWork.Save();

                serviceResponse.Data = profileImage;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        // public async Task<ServiceResponse<BetVm>> UpdateBet(BetVm betVm)
        // {
        //     var serviceResponse = new ServiceResponse<BetVm>();
        //
        //     try
        //     {
        //         var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == betVm.CreatedBy.Id, includeProperties: "UserProfile");
        //         if (foundUser == null)
        //         {
        //             serviceResponse.Success = false;
        //             serviceResponse.Message = "No User Found by Id";
        //             return serviceResponse;
        //         }
        //
        //         var foundBet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == betVm.BetId);
        //         if (foundBet == null)
        //         {
        //             serviceResponse.Success = false;
        //             serviceResponse.Message = "No Bet Found by Id";
        //             return serviceResponse;
        //         }
        //
        //         foundBet.Title = betVm.Title;
        //         foundBet.Description = betVm.Description;
        //         foundBet.AcceptedByUserIds = betVm.AcceptedBy.Select(u => u.Id).ToList();
        //         foundBet.Amount = betVm.Amount;
        //         foundBet.CanAcceptNumber = betVm.CanAcceptNumber;
        //         foundBet.UpdatedOn = DateTime.UtcNow;
        //         foundBet.IsActive = betVm.IsActive;
        //         foundBet.DoesRequirePassCode = betVm.DoesRequirePassCode;
        //         await _unitOfWork.Save();
        //
        //
        //         var bvm = await BetMapper.BetVm(foundBet);
        //         serviceResponse.Data = bvm;
        //     }
        //     catch (Exception e)
        //     {
        //         serviceResponse.Message = e.Message;
        //         serviceResponse.Success = false;
        //     }
        //
        //     return serviceResponse;
        // }

        public async Task<ServiceResponse<string>> DeleteBet(BetVm betVm)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.Id == betVm.CreatedBy.Id, includeProperties: "UserProfile");
                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No User Found by Id";
                    return serviceResponse;
                }

                var foundBet = await _unitOfWork.Bet.GetFirstOrDefault(b => b.BetId == betVm.BetId);
                if (foundBet == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "No Bet Found by Id";
                    return serviceResponse;
                }

                await _unitOfWork.Bet.RemoveAsync(foundBet.BetId);

                serviceResponse.Data = "Bet has been deleted";
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
