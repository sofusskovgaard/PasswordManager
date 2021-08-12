using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Data.Entities.User;

namespace PasswordManager.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken CreateToken(UserEntity user, bool longSession = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = _configuration.GetSection("Jwt").GetValue<string>("secret");
            var secretBytes = Encoding.ASCII.GetBytes(secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = longSession ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            
            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }
    }
}