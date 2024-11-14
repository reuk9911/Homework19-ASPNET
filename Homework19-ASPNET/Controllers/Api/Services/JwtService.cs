using Homework19_ASPNET.Auth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using Homework19_ASPNET.Controllers.Api.Models;


namespace Homework19_ASPNET.Controllers.Api.Services
{
    public interface ITokenService
    {
        //public string GetAccessToken(/*IEnumerable<Claim> claims, out DateTime expires*/);
        public string GetAccessToken(Account account);
        public string GetRefreshToken();
        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class JwtService:ITokenService
    {
        private IOptions<AuthSettings> _options;
        public JwtService(IOptions<AuthSettings> options) 
        {
            _options = options;
        }
        public string GetAccessToken(Account account) 
        {
            var claims = new List<Claim>
            {
                new Claim("login", account.UserName),
                new Claim("id", account.Id.ToString()),
            };
            foreach (var role in account.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(_options.Value.Expires),
                claims: claims,
                signingCredentials:new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        //{
        //    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        //    TokenValidationParameters validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = _options.ValidateIssuer,
        //        ValidateAudience = _options.ValidateAudience,
        //        ValidateIssuerSigningKey = _options.ValidateIssuerSigningKey,
        //        ValidIssuer = _options.Issuer,
        //        ValidAudience = _options.Audience,
        //        IssuerSigningKey = key,
        //    };
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
        //    var jwtSecurityToken = securityToken as JwtSecurityToken;
        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        throw new SecurityTokenException("Invalid token");
        //    return principal;
        //}


        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber);
        }

    }
}
