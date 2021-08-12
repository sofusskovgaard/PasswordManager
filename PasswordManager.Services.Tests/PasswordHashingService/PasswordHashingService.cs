using Xunit;
using Xunit.Abstractions;

namespace PasswordManager.Services.Tests.PasswordHashingService
{
    public class PasswordHashingService
    {
        private readonly ITestOutputHelper output;

        public PasswordHashingService(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CreateHash()
        {
            var passwordHashingService = new Services.PasswordHashingService.PasswordHashingService();

            var value = "P@ssw0rd!";
            output.WriteLine($"Value: {value}\n");

            var hash = passwordHashingService.CreateHash(value);
            output.WriteLine($"Hash: {hash}\n");
            
            Assert.NotEqual(value, hash);
        }
        
        [Fact]
        public void VerifyHash()
        {
            var passwordHashingService = new Services.PasswordHashingService.PasswordHashingService();

            var value = "P@ssw0rd!";
            output.WriteLine($"Value: {value}\n");

            var hash = passwordHashingService.CreateHash(value);
            output.WriteLine($"Hash: {hash}\n");

            var verify = passwordHashingService.VerifyHash(value, hash);
            output.WriteLine($"Valid: {verify}");
            
            Assert.True(verify);
        }
        
        [Fact]
        public void VerifyBadHash()
        {
            var passwordHashingService = new Services.PasswordHashingService.PasswordHashingService();

            var value = "P@ssw0rd!";
            output.WriteLine($"Value: {value}\n");

            var hash = passwordHashingService.CreateHash(value) + "poopie";
            output.WriteLine($"Hash: {hash}\n");

            var verify = passwordHashingService.VerifyHash(value, hash);
            output.WriteLine($"Valid: {verify}");
            
            Assert.False(verify);
        }
    }
}