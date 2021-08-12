using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PasswordManager.Common.Commands;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.User;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Account
{
    public class Index : InjectedPageModel
    {
        private readonly IDataAccessService _dataAccessService;
        
        public Index(IAuthenticationService authenticationService, IDataAccessService dataAccessService) : base(authenticationService)
        {
            _dataAccessService = dataAccessService;
        }
        
        [BindProperty]
        public UpdateUserCommand Form { get; set; }
        
        public Task<IActionResult> OnGet()
        {
            return this.Try(() =>
            {
                Form = new UpdateUserCommand()
                {
                    Firstname = this.CurrentUser.FirstName,
                    Lastname = this.CurrentUser.LastName,
                    Email = this.CurrentUser.EMail
                };
                
                return Page();
            }, true);
        }

        public Task<IActionResult> OnPost()
        {
            return this.TryAsync(async () =>
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var user = await _dataAccessService.Get<UserEntity>(this.CurrentUser.Id);
                
                user.FirstName = Form.Firstname;
                user.LastName = Form.Lastname;
                user.EMail = Form.Email;

                await _dataAccessService.Update(user);
                
                return RedirectToPage();
            }, true);
        }
    }
}