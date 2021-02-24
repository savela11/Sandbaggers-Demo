using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DTO;
using Models.ViewModels;
using Services.Interface;
using Utilities;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettingsExtension _appSettings;

        public AuthService(IOptions<AppSettingsExtension> appSettings, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
        }


        public async Task<ServiceResponse<List<string>>> RegisterMultipleUsers(List<RegisterUserDto> registerUserDtoList)
        {
            var serviceResponse = new ServiceResponse<List<string>>();

            try
            {
                var userNames = new List<string>();
                foreach (var registerUserDto in registerUserDtoList)
                {
                    var res = await RegisterUser(registerUserDto);
                    userNames.Add(res.Data.Username);
                }

                serviceResponse.Data = userNames;
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ApplicationUserVm>> RegisterUser(RegisterUserDto registerUserDto)
        {
            var serviceResponse = new ServiceResponse<ApplicationUserVm>();
            if (registerUserDto.RegistrationCode != "2006" && registerUserDto.RegistrationCode != "ADMIN2006" && registerUserDto.RegistrationCode != "TestUser")
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Incorrect Registration Code...";
                return serviceResponse;
            }

            try
            {
                var newUser = new ApplicationUser
                {
                    UserName = registerUserDto.UserName, Email = registerUserDto.Email, PhoneNumber = "", FullName = registerUserDto.FirstName + " " + registerUserDto.LastName
                };
                var result = await _unitOfWork.UserManager.CreateUser(newUser, registerUserDto.Password);
                if (result.Succeeded)
                {
                    var newProfile = new UserProfile
                    {
                        UserId = newUser.Id,
                        FirstName = registerUserDto.FirstName,
                        LastName = registerUserDto.LastName,
                        User = newUser,
                        Handicap = 0,
                        Image = "https://sbassets.blob.core.windows.net/default/defaultProfileImg.svg",
                        CreatedOn = DateTime.UtcNow,
                    };


                    var favoriteLinks = new List<FavoriteLink>
                    {
                        new FavoriteLink
                        {
                            Link = "/dashboard",
                            Name = "Dashboard",
                        },
                        new FavoriteLink
                        {
                            Link = "/bets",
                            Name = "Bets",
                        },

                        new FavoriteLink
                        {
                            Link = "/contacts",
                            Name = "Contacts",
                        }
                    };
                    var newSettings = new UserSettings {UserId = newUser.Id, FavoriteLinks = favoriteLinks};
                    newUser.UserProfile = newProfile;
                    newUser.UserSettings = newSettings;

                    string roleName;
                    string claimValue;
                    // Check if Admin or User role exists before adding a user to that role
                    if (registerUserDto.RegistrationCode == "ADMIN2006" || registerUserDto.RegistrationCode == "2006")
                    {
                        roleName = registerUserDto.RegistrationCode == "ADMIN2006" ? "Admin" : "User";
                        claimValue = "User";
                    }
                    else
                    {
                        roleName = "TestUser";

                        claimValue = "TestUser";
                    }

                    if (roleName == "Admin")
                    {
                        var doesAdminRoleExist = await _unitOfWork.RoleManager.DoesRoleExist("Admin");
                        if (doesAdminRoleExist == false)
                        {
                            await _unitOfWork.RoleManager.CreateRole("Admin");
                        }

                        await _unitOfWork.UserManager.AddUserToRole(newUser, "Admin");
                    }

                    // Check if User Role Exists
                    var doesUserRoleExist = await _unitOfWork.RoleManager.DoesRoleExist("User");
                    if (doesUserRoleExist == false)
                    {
                        await _unitOfWork.RoleManager.CreateRole("User");
                    }

                    await _unitOfWork.UserManager.AddUserToRole(newUser, "User");
                    await _unitOfWork.UserManager.CreateNewClaim(newUser, claimValue);
                    await _unitOfWork.Save();
                    var applicationUserVm = new ApplicationUserVm
                    {
                        Email = newUser.Email,
                        Id = newUser.Id,
                        Username = newUser.UserName,
                        PhoneNumber = newUser.PhoneNumber
                    };
                    serviceResponse.Data = applicationUserVm;
                }
                else
                {
                    List<string> errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }

                    serviceResponse.Errors = errorList;
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Error during Registration";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Message = e.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<LoggedInUserVm>> LoginUser(LoginUserDto loginUserDto)
        {
            var serviceResponse = new ServiceResponse<LoggedInUserVm>();
            try
            {
                var foundUser = await _unitOfWork.User.GetFirstOrDefault(u => u.NormalizedUserName == loginUserDto.UserName.ToUpper(), "UserProfile,UserSettings");

                if (foundUser == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Username {loginUserDto.UserName} not found. ";
                    return serviceResponse;
                }

                var signInResult = await _unitOfWork.SignInManager.SignInUser(loginUserDto.UserName, loginUserDto.Password);

                if (signInResult.Succeeded)
                {
                    var userRoles = await _unitOfWork.UserManager.UserRoles(foundUser);
                    var token = await AuthenticateUser(foundUser, userRoles);
                    var loggedInUserVm = new LoggedInUserVm
                    {
                        Id = foundUser.Id,
                        Username = foundUser.UserName,
                        Roles = userRoles.ToList(),
                        Token = token,

                        Settings = new UserSettingsVm
                        {
                            FavoriteLinks = foundUser.UserSettings.FavoriteLinks.Select(l => new FavoriteLinkVm {Link = l.Link, Name = l.Name}).ToList(),
                            IsContactEmailShowing = foundUser.UserSettings.IsContactEmailShowing,
                            IsContactNumberShowing = foundUser.UserSettings.IsContactNumberShowing
                        },

                        Image = foundUser.UserProfile.Image
                    };


                    serviceResponse.Data = loggedInUserVm;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Check username / password.";
                }
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> LogoutUser()
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                await _unitOfWork.SignInManager.SignOutUser();
                serviceResponse.Data = "User successfully signed out.";
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }


        private async Task<string> AuthenticateUser(ApplicationUser user, ICollection<string> userRoles)
        {
            if (user == null)
            {
                return null;
            }

            var role = userRoles.Contains("Admin") ? "Admin" : "User";
            var userClaims = await _unitOfWork.UserManager.GetClaims(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(userClaims[0].Type, userClaims[0].Value),
                    new Claim(ClaimTypes.Name, user.Id),
                    // new Claim(ClaimsRole, "Admin"),
                    // new Claim("TestClaim", "Test"),
                    new Claim(ClaimTypes.Role, role)
                }),

                // Issuer = api website that issued the token
                // Audience - who the token is supposed to be read by
                Expires = DateTime.UtcNow.AddDays(1),
                // Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var userToken = tokenHandler.WriteToken(token);

            return userToken;
        }
    }
}
