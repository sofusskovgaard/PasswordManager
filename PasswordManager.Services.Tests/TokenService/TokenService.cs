using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using PasswordManager.Data.Entities.User;
using PasswordManager.TestUtils;
using Xunit;
using Xunit.Abstractions;

namespace PasswordManager.Services.Tests.TokenService
{
    public class TokenService
    {
        private readonly ITestOutputHelper output;

        public TokenService(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CreateToken()
        {
            var tokenService = new Services.TokenService.TokenService(MockUtil.CreateConfiguration());
            var tokenHandler = new JwtSecurityTokenHandler();

            var expectedId = "123";
            
            var user = new UserEntity()
            {
                Id = expectedId
            };

            var jwt = tokenService.CreateToken(user);
            var tokenString = tokenHandler.WriteToken(jwt);
            output.WriteLine($"Token: {tokenString}");

            var token = new JwtSecurityToken(tokenString);
            
            Assert.Equal(token.Claims.First(x => x.Type == "id").Value, expectedId);
        }
        
        [Fact]
        public void ValidateToken()
        {
            var tokenService = new Services.TokenService.TokenService(MockUtil.CreateConfiguration());
            var tokenHandler = new JwtSecurityTokenHandler();

            var user = new UserEntity()
            {
                Id = "123"
            };

            var jwt = tokenService.CreateToken(user);
            var tokenString = tokenHandler.WriteToken(jwt);
            var tokenValid = tokenService.ValidateToken(tokenString);
            
            Assert.True(tokenValid);
        }
        
        [Fact]
        public void ValidateBadToken()
        {
            var tokenService = new Services.TokenService.TokenService(MockUtil.CreateConfiguration());
            var tokenHandler = new JwtSecurityTokenHandler();

            var user = new UserEntity()
            {
                Id = "123"
            };

            var jwt = tokenService.CreateToken(user);
            var tokenString = tokenHandler.WriteToken(jwt) + "poopie";
            var tokenValid = tokenService.ValidateToken(tokenString);
            
            Assert.False(tokenValid);
        }
    }
}