using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.Password;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Services.CryptographyService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages.Passwords
{
    public class Index : InjectedPageModel
    {
        private readonly ICryptographyService _cryptographyService;

        private readonly IDataAccessService _dataAccessService;
        public Index(IAuthenticationService authenticationService, ICryptographyService cryptographyService, IDataAccessService dataAccessService) : base(authenticationService)
        {
            _cryptographyService = cryptographyService;
            _dataAccessService = dataAccessService;
        }
        
        [BindProperty]
        public IEnumerable<PasswordEntity> Passwords { get; set; }
        
        public Task<IActionResult> OnGet()
        {
            return this.TryAsync(async () =>
            {
                var passwords = await _dataAccessService.GetAll(Builders<PasswordEntity>.Filter.Eq(x => x.OwnerId, this.CurrentUser.Id));
                Passwords = new List<PasswordEntity>(passwords);
                return Page();
            }, true);
        }

        public Task<IActionResult> OnPostRemove(string id)
        {
            return this.TryAsync(async () =>
            {
                var entity = await _dataAccessService.Get<PasswordEntity>(id);
                await _dataAccessService.Delete(entity);
                return RedirectToPage();
            }, true);
        }
    }
}