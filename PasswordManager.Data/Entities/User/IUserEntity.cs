namespace PasswordManager.Data.Entities.User
{
    public interface IUserEntity : IBaseEntity
    {
        string FirstName { get; set; }
        
        string LastName { get; set; }
        
        string EMail { get; set; }
        
        string Password { get; set; }
        
        string PasswordHash { get; }
        
        string PasswordSalt { get; }
    }
}