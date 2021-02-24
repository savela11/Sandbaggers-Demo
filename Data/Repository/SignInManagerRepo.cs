using System.Threading.Tasks;
using Data.Models;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository
{
    public class SignInManagerRepo : ISignInManagerRepo
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInManagerRepo(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }


        public async Task<SignInResult> SignInUser(string username, string password)
                {
                    return await _signInManager.PasswordSignInAsync(username,
                        password, false,
                        lockoutOnFailure: false);
                }


        public async Task SignOutUser()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
