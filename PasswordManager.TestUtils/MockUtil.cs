using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Moq;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Services.CryptographyService;
using PasswordManager.Services.PasswordHashingService;
using PasswordManager.Services.TokenService;

namespace PasswordManager.TestUtils
{
    public static class MockUtil
    {
        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
        }

        public static Mock<ITokenService> CreateTokenService()
        {
            return new Mock<ITokenService>(CreateConfiguration()) { CallBase = true };
        }
        
        public static Mock<IPasswordHashingService> CreatePasswordHashingService()
        {
            return new Mock<IPasswordHashingService>() { CallBase = true };
        }
        
        public static Mock<ICryptographyService> CreateCryptographyService()
        {
            return new Mock<ICryptographyService>() { CallBase = true };
        }
        
        public static Mock<IDataAccessService> CreateDataAccessService()
        {
            return new Mock<IDataAccessService>(CreateConfiguration()) { CallBase = true };
        }
    }
}