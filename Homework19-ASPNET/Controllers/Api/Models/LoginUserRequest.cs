namespace Homework19_ASPNET.Controllers.Api.Models
{
    public class LoginUserRequest
    {
        public string userName { get; set; }
        public string hashedPassword { get; private set; }
        
        public LoginUserRequest(string userName, string password)
        {
            this.userName = userName;
            this.hashedPassword = password; // !!!
        }
    }
}
