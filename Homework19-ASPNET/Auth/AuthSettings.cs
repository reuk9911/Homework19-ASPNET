using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Homework19_ASPNET.Auth
{
    public class AuthSettings
    {
        //public const string ISSUER = "MyAuthServer"; // издатель токена
        //public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        //const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public TimeSpan Expires { get; set; } // время жизни токена
        public string SecretKey { get; set; }
        //public static SymmetricSecurityKey GetSymmetricSecurityKey()
        //{
        //    return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        //}
    }
}
