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
        public string GetAccessToken(string userName, List<string> roles);
        public string GetRefreshToken();
        //public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }

    public class JwtService:ITokenService
    {
        private IOptions<AuthSettings> _settings;
        public JwtService(IOptions<AuthSettings> settings) 
        {
            _settings = settings;
        }
        public string GetAccessToken(string userName, List<string> roles) 
        {
            var claims = new List<Claim>
            {
                new Claim("login", userName),
            };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(_settings.Value.Expires),
                claims: claims,
                signingCredentials:new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256)
                );
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }

        /// <summary>
        /// Неповторимый оригинал
        /// </summary>
        /*public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _options.ValidateIssuer,
                ValidateAudience = _options.ValidateAudience,
                ValidateIssuerSigningKey = _options.ValidateIssuerSigningKey,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = key,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }*/


        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.SecretKey));
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                //ValidateIssuer = _settings.ValidateIssuer,
                //ValidateAudience = _settings.ValidateAudience,
                //ValidateIssuerSigningKey = _settings.ValidateIssuerSigningKey,
                //ValidIssuer = _settings.Issuer,
                //ValidAudience = _settings.Audience,
                IssuerSigningKey = key,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }


        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToHexString(randomNumber);
        }

    }
}
