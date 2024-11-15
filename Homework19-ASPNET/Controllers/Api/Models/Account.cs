namespace Homework19_ASPNET.Controllers.Api.Models
{
    /// <summary>
    /// Аккаунт из БД 
    /// можно удалить
    /// </summary>
    public class Account
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }

        public Account(string login, List<string> roles)
        {
            UserName = login;
            Roles = roles;
        }
    }
}
