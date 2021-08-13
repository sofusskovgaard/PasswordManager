using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Services.PasswordHashingService
{
    public class PasswordHashingService : IPasswordHashingService
    {
        public string CreateHash(string password)
        {
            var salt = _generateSalt();
            var hash = _generateHash(password, salt);
            return $"{Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public bool VerifyHash(string password, string saltAndHashString)
        {
            var splat = saltAndHashString.Split("$");
            var salt = splat.First();
            var hash = splat.Last();

            var newHash = _generateHash(password, Convert.FromBase64String(salt));

            return Convert.ToBase64String(newHash) == hash;
        }
        
        private byte[] _generateSalt()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            
            var randomNumber = new byte[32];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        private byte[] _generateHash(string password, byte[] salt)
        {
            using var hash = new Rfc2898DeriveBytes(password, salt, 696969);
            return hash.GetBytes(64);
        }
    }
}