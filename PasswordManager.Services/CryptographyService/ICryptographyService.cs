namespace PasswordManager.Services.CryptographyService
{
    public interface ICryptographyService
    {
        string Encrypt(string content);
        
        string Decrypt(string content);
    }
}