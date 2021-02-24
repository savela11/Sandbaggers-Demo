using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class UserMapper
    {
        public static IUnitOfWork OfWork { get; }

        public static UserVm UserVm(ApplicationUser user)
        {
            return new UserVm
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                Profile = UserProfileMapper.UserProfileVm(user.UserProfile),
                Settings = UserSettingsMapper.UserSettingsVm(user.UserSettings)
            };
        }

        public static CreatedByUserVm CreatedByUserVm(ApplicationUser user)
        {
            return new CreatedByUserVm
            {
                Id = user.Id,
                FullName = user.FullName,
                Image = user.UserProfile.Image
            };
        }


        public static AcceptedByUserVm AcceptedByUserVm(ApplicationUser user)
        {
            return new AcceptedByUserVm
            {
                Id = user.Id,
                Image = user.UserProfile.Image,
                FullName = user.FullName
            };
        }

        public static List<AcceptedByUserVm> AcceptedByUserVmList(IEnumerable<ApplicationUser> users)
        {
            var acceptedByUserVmList = new List<AcceptedByUserVm>();

            foreach (var user in users)
            {
                var acceptedByUserVm = AcceptedByUserVm(user);

                acceptedByUserVmList.Add(acceptedByUserVm);
            }

            return acceptedByUserVmList;
        }

        public static List<RegisteredUserVm> RegisteredUserVmList(IEnumerable<ApplicationUser> users)
        {
            var registeredUserVmList = new List<RegisteredUserVm>();
            foreach (var user in users)
            {
                var registeredUserVm = RegisteredUserVm(user);

                registeredUserVmList.Add(registeredUserVm);
            }

            return registeredUserVmList;
        }

        public static RegisteredUserVm RegisteredUserVm(ApplicationUser user)
        {
            return new RegisteredUserVm
            {
                Id = user.Id,
                Username = user.UserName,
                FullName = user.FullName,
                Image = user.UserProfile.Image
            };
        }


        public static async Task<List<ScrambleChampVm>> ScrambleChampVms(IEnumerable<string> scrambleChampIds)
        {
            var scrambleChampVmList = new List<ScrambleChampVm>();

            foreach (var champId in scrambleChampIds)
            {
                var scrambleChampVm = await ScrambleChampVm(champId);

                scrambleChampVmList.Add(scrambleChampVm);
            }

            return scrambleChampVmList;
        }

        public static async Task<ScrambleChampVm> ScrambleChampVm(string id)
        {
            var foundUser = await OfWork.User.GetFirstOrDefault(u => u.Id == id, "UserProfile");

            return new ScrambleChampVm
            {
                Id = foundUser.Id,
                FullName = foundUser.FullName,
                Image = foundUser.UserProfile.Image
            };
        }
    }
}
