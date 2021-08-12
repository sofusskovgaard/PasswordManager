namespace PasswordManager.Services.PasswordHashingService
{
    public interface IPasswordHashingService
    {
        string CreateHash(string password);
        
        bool VerifyHash(string password, string hash);
    }
}