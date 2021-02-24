using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Repository.Interface
{
    public interface ISignInManagerRepo
    {
        Task<SignInResult> SignInUser(string username, string password);

        Task SignOutUser();
    }
}
