namespace PasswordManager.Common.Commands
{
    public class LoginCommand
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public bool StayLoggedIn { get; set; }
    }
}