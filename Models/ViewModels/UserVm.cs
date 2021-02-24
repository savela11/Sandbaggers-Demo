using System.Collections.Generic;

namespace Models.ViewModels
{
    public class ApplicationUserVm
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserVm : ApplicationUserVm
    {
        public string FullName { get; set; }
        public UserProfileVm Profile { get; set; }
        public UserSettingsVm Settings { get; set; }
    }


    public class LoggedInUserVm
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }

        public UserSettingsVm Settings { get; set; }

        public string Image { get; set; }
    }

    public class CreatedByUserVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }

    }

    public class AcceptedByUserVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }

        public string Image { get; set; }
    }
}
