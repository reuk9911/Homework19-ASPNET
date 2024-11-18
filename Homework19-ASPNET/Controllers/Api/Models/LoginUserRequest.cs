using Microsoft.AspNetCore.Identity;

namespace Homework19_ASPNET.Controllers.Api.Models
{
    public class LoginUserRequest
    {
        public string UserName { get; set; }
        public string Password { get; private set; }
        
        public LoginUserRequest(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}
