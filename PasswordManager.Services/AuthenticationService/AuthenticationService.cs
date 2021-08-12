using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using PasswordManager.Common.Commands;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.User;
using PasswordManager.Services.PasswordHashingService;
using PasswordManager.Services.TokenService;

namespace PasswordManager.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDataAccessService _dataAccessService;

        private readonly ITokenService _tokenService;

        private readonly IPasswordHashingService _passwordHashingService;

        public AuthenticationService(IDataAccessService dataAccessService, ITokenService tokenService, IPasswordHashingService passwordHashingService)
        {
            _dataAccessService = dataAccessService;
            _tokenService = tokenService;
            _passwordHashingService = passwordHashingService;
        }

        public async Task<JwtSecurityToken> Login(LoginCommand command)
        {
            var user = await _dataAccessService.Get<UserEntity>(Builders<UserEntity>.Filter.Eq(x => x.EMail, command.Email));
            
            if (user == null)
            {
                throw new ArgumentException("user does not exist");
            }

            if (_passwordHashingService.VerifyHash(command.Password, user.Password))
            {
                var token = _tokenService.CreateToken(user);
                return token; 
            }
                
            throw new ArgumentException("password does not match");
        }

        public async Task<JwtSecurityToken> Register(RegisterCommand command)
        {
            if (command.Password != command.ConfirmPassword)
            {
                throw new ArgumentException("passwords do not match");
            }
            
            var passwordHash = _passwordHashingService.CreateHash(command.Password);
            
            var user = new UserEntity()
            {
                FirstName = command.Firstname,
                LastName = command.Lastname,
                EMail = command.Email,
                Password = passwordHash
            };

            await _dataAccessService.Create(user);
            
            return _tokenService.CreateToken(user);
        }
    }
}