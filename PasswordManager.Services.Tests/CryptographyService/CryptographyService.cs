using Xunit;
using Xunit.Abstractions;

namespace PasswordManager.Services.Tests.CryptographyService
{
    public class CryptographyService
    {
        private readonly ITestOutputHelper output;

        public CryptographyService(ITestOutputHelper output)
        {
            this.output = output;
            
            // Ensure encryption keys exist.
            Services.CryptographyService.CryptographyService.CreateEncryptionKeys();
        }

        [Fact]
        public void Encryption()
        {
            var cryptographyService = new Services.CryptographyService.CryptographyService();

            var expectedAnswer =
                "According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyway because bees don't care what humans think is impossible.";
            output.WriteLine($"Expected answer: {expectedAnswer}\n");
            
            var encrypted = cryptographyService.Encrypt(expectedAnswer);
            output.WriteLine($"Encrypted answer: {encrypted}\n");
            
            Assert.NotEqual(expectedAnswer, encrypted);
        }

        [Fact]
        public void EncryptionAndDecryption()
        {
            var cryptographyService = new Services.CryptographyService.CryptographyService();

            var expectedAnswer =
                "According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyway because bees don't care what humans think is impossible.";
            output.WriteLine($"Expected answer: {expectedAnswer}\n");
            
            var encrypted = cryptographyService.Encrypt(expectedAnswer);
            output.WriteLine($"Encrypted answer: {encrypted}\n");

            var decrypted = cryptographyService.Decrypt(encrypted);
            output.WriteLine($"Decrypted answer: {decrypted}");
            
            Assert.Equal(expectedAnswer, decrypted);
        }
    }
}