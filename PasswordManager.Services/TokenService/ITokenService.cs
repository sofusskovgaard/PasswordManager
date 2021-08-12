using System.IdentityModel.Tokens.Jwt;
using PasswordManager.Data.Entities.User;

namespace PasswordManager.Services.TokenService
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(UserEntity user, bool longSession = false);

        bool ValidateToken(string token);
    }
}