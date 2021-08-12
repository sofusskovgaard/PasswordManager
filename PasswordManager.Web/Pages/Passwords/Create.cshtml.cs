using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using PasswordManager.Common.Commands;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.Password;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Services.CryptographyService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Passwords
{
    public class Create : InjectedPageModel
    {
        private readonly ICryptographyService _cryptographyService;

        private readonly IDataAccessService _dataAccessService;
        
        public Create(IAuthenticationService authenticationService, ICryptographyService cryptographyService, IDataAccessService dataAccessService) : base(authenticationService)
        {
            _cryptographyService = cryptographyService;
            _dataAccessService = dataAccessService;
        }
        
        [BindProperty]
        public CreatePasswordCommand Form { get; set; }

        public Task<IActionResult> OnGet()
        {
            return this.Try(Page, true);
        }

        public Task<IActionResult> OnPost()
        {
            return this.TryAsync(async () =>
            {
                var encryptedPassword = _cryptographyService.Encrypt(Form.Password);
                
                var entity = new PasswordEntity()
                {
                    Name = Form.Name,
                    URL = Form.URL,
                    Username = Form.Username,
                    Password = encryptedPassword,
                    OwnerId = this.CurrentUser.Id
                };

                await _dataAccessService.Create(entity);
                
                return RedirectToPage("Index");
            }, true);
        }
    }
}