using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class UserProfileMapper
    {
        public static UserProfile UserProfile(UserProfileVm userProfileVm)
        {
            return new UserProfile
            {
                Image = userProfileVm.Image,
                FirstName = userProfileVm.FirstName,
                LastName = userProfileVm.LastName,
                Handicap = userProfileVm.Handicap
            };
        }

        public static UserProfileVm UserProfileVm(UserProfile userProfile)
        {
            return new UserProfileVm
            {
                Image = userProfile.Image,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Handicap = userProfile.Handicap
            };
        }
    }
}
