using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Auth
{
    public class Logout : InjectedPageModel
    {
        public Logout(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }
        
        public IActionResult OnGet()
        {
            Response.Cookies.Delete("Authorization");
            return RedirectToPage("../Index");
        }
    }
}