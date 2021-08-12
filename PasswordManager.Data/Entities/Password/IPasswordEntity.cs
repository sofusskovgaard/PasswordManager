namespace PasswordManager.Data.Entities.Password
{
    public interface IPasswordEntity : IBaseEntity
    {
        string URL { get; set; }
        
        string Name { get; set; }
        
        string Username { get; set; }
        
        string Password { get; set; }
    }
}