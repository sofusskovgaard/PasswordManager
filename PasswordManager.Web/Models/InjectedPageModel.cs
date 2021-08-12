using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.User;
using PasswordManager.Services.AuthenticationService;

namespace PasswordManager.Web.Models
{
    public abstract class InjectedPageModel : PageModel
    {
        public readonly IAuthenticationService AuthenticationService;

        protected InjectedPageModel(IAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        public UserEntity CurrentUser => (UserEntity)HttpContext.Items["User"];

        public bool LoggedIn => CurrentUser != null;

        public async Task<IActionResult> Try(Func<IActionResult> stuffToTry, bool shouldBeAuthenticated = false)
        {
            try
            {
                if (shouldBeAuthenticated && !this.LoggedIn)
                {
                    return RedirectToPage("/Auth/Login");
                }
                
                return await Task.Run(stuffToTry);
            }
            catch (Exception e)
            {
                return RedirectToPage("/Error");
            }
        }
        
        public async Task<IActionResult> TryAsync(Func<Task<IActionResult>> stuffToTry, bool shouldBeAuthenticated = false)
        {
            try
            {
                if (shouldBeAuthenticated && !this.LoggedIn)
                {
                    return RedirectToPage("/Auth/Login");
                }

                return await stuffToTry();
            }
            catch (Exception e)
            {
#if DEBUG
                throw;
#else
                return RedirectToPage("/Error");
#endif
            }
        }
    }
}