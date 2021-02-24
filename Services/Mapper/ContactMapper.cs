using System.Collections.Generic;
using Data.Models;
using Models.ViewModels;

namespace Services.Mapper
{
    public static class ContactMapper
    {
        public static List<ContactVm> ContactVmList(IEnumerable<ApplicationUser> users)
        {
            var contactVmList = new List<ContactVm>();

            foreach (var user in users)
            {
                var contactVm = ContactVm(user);

                contactVmList.Add(contactVm);
            }


            return contactVmList;
        }


        public static ContactVm ContactVm(ApplicationUser user)
        {
            return new ContactVm
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Image = user.UserProfile.Image,
                IsContactNumberShowing = user.UserSettings.IsContactNumberShowing,
                IsContactEmailShowing = user.UserSettings.IsContactEmailShowing,
            };
        }
    }
}
