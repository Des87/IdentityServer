using IdentityServer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace IdentityServer.Helper
{
    public static class TokenHelper
    {
        public static string GenerateJSONWebToken(User user)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string url = MyConfig.GetValue<string>("applicationUrl");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismySecretKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };
            var token = new JwtSecurityToken(url,
              url,
              claims,
              expires: DateTime.UtcNow.AddDays(2),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GetUserFromToken(string token)
        {
            try
            {
                var jwtSecurityToken = new JwtSecurityToken(token);
                if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
                {
                    throw new SecurityTokenInvalidLifetimeException();
                }
                var claims = jwtSecurityToken.Claims.ToList();

                return claims[0].Value;
            }
            catch (SecurityTokenInvalidLifetimeException ex) 
            { 
                throw new SecurityTokenInvalidLifetimeException("The token expire is over"); 
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

        }
    }
}
