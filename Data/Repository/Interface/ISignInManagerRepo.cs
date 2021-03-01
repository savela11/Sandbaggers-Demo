using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Interface
{
    public interface ISignInManagerRepo
    {
        Task<SignInResult> SignInUser(string username, string password);

        Task SignOutUser();


        Task<List<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool rememberMe);
    }
}
