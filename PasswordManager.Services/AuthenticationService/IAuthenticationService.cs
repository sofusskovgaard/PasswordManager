using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;
using PasswordManager.Common.Commands;
using PasswordManager.Data.Entities.User;

namespace PasswordManager.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<JwtSecurityToken> Login(LoginCommand command);

        Task<JwtSecurityToken> Register(RegisterCommand command);
    }
}