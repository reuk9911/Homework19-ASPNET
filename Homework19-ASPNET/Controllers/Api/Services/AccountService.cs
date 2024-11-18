using Homework19_ASPNET.Auth;
using Homework19_ASPNET.Controllers.Api.Models;
using Homework19_ASPNET.Data;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Homework19_ASPNET.Controllers.Api.Services
{
    public class AccountService
    {

        private readonly UserManager<User> _userManager;
        private IdentityError[] _errors;
        private JwtService _jwtService;

        public AccountService(UserManager<User> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _errors = new[]
                {
                    new IdentityError{ Code="0", Description="User already exists"},
                    new IdentityError{ Code="1", Description="User registration failed" },
                    new IdentityError{ Code="2", Description="User not found" },
                    new IdentityError{ Code="3", Description="Incorrect password" }
                };
        }

        public async Task<IdentityResult> Register(string userName, string password, string role)
        {
            if (await _userManager.FindByNameAsync(userName) == null)
            {
                User user = new User { UserName = userName };
                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                    return IdentityResult.Success;
                }
                else
                {

                    return IdentityResult.Failed(_errors[1]); ;
                }
            }
            else
            {
                return IdentityResult.Failed(_errors[0]);
            }
        }

        public string Login(string userName, string password)
        {
            User? user = _userManager.Users.FirstOrDefault<User>(p => p.UserName == userName);

            if (user == null)
                return "User not found";
            else
            {
                var result = new PasswordHasher<User>().
                    VerifyHashedPassword(user, user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    var task = _userManager.GetRolesAsync(user).Result;
                    List<string>? roles = task.ToList<string>();
                    var token = _jwtService.GetAccessToken(user.UserName, roles);
                    return token;
                }
                else
                    return "Wrong username or password";
            }

        }

    }
}
