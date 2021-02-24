using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public UserProfile UserProfile { get; set; }

        public UserSettings UserSettings { get; set; }

    }
}
