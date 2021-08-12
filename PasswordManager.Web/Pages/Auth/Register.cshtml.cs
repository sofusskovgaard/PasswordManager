using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordManager.Common.Commands;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Auth
{
    public class Register : InjectedPageModel
    {
        public Register(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }
        
        [BindProperty]
        public RegisterCommand RegisterForm { get; set; }
        
        public IActionResult OnGet()
        {
            if (LoggedIn)
            {
                return RedirectToPage("../Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var token = await AuthenticationService.Register(RegisterForm);
                if (token != null)
                {
                    var handler = new JwtSecurityTokenHandler();
                    Response.Cookies.Append("Authorization", $"Bearer {handler.WriteToken(token)}");

                    return RedirectToPage();
                }
            }

            return Page();
        }
    }
}