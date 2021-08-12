using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace PasswordManager.Services.CryptographyService
{
    public class CryptographyService : ICryptographyService
    {
        private static string PublicKey => Directory.GetParent(Environment.CurrentDirectory)?.FullName + "\\keys\\public.xml";
        private static string PrivateKey => Directory.GetParent(Environment.CurrentDirectory)?.FullName + "\\keys\\private.xml";

        public string Encrypt(string content)
        {
            var encrypted = _encrypt(Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string content)
        {
            var decrypted = _decrypt(Convert.FromBase64String(content));
            return Encoding.UTF8.GetString(decrypted);
        }

        private byte[] _encrypt(byte[] content)
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            
            rsa.PersistKeyInCsp = false;                
            rsa.FromXmlString(File.ReadAllText(PublicKey));

            return rsa.Encrypt(content, false);
        }
        
        private byte[] _decrypt(byte[] content)
        {
            using var rsa = new RSACryptoServiceProvider(2048);

            rsa.PersistKeyInCsp = false;
            rsa.FromXmlString(File.ReadAllText(PrivateKey));
            
            return rsa.Decrypt(content, false);
        }

        public static void CreateEncryptionKeys()
        {
            if (File.Exists(PublicKey) && File.Exists(PrivateKey)) return;
            
            using var rsa = new RSACryptoServiceProvider(2048);
            
            rsa.PersistKeyInCsp = false;

            if (File.Exists(PrivateKey))
            {
                File.Delete(PrivateKey);
            }

            if (File.Exists(PublicKey))
            {
                File.Delete(PublicKey);
            }

            var publicKeyfolder = Path.GetDirectoryName(PublicKey);
            var privateKeyfolder = Path.GetDirectoryName(PrivateKey);

            if (!Directory.Exists(publicKeyfolder))
            {
                Directory.CreateDirectory(publicKeyfolder);
            }

            if (!Directory.Exists(privateKeyfolder))
            {
                Directory.CreateDirectory(privateKeyfolder);
            }
           
            File.WriteAllText(PublicKey, rsa.ToXmlString(false));
            File.WriteAllText(PrivateKey, rsa.ToXmlString(true));
        }
    }
}