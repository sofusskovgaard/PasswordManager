using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Passwords
{
    public class Index : InjectedPageModel
    {
        public Index(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }
        
        public Task<IActionResult> OnGet()
        {
            return this.Try(Page, true);
        }
    }
}