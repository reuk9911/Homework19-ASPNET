namespace Homework19_ASPNET.Controllers.Api.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Id { get; private set; }

        public Account(string login, string id, List<string> roles)
        {
            UserName = login;
            Id = id;
            Roles = roles;
        }
    }
}
