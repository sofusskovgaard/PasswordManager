using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PasswordManager.Common.Commands;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Web.Models;

namespace PasswordManager.Web.Pages
{
    public class IndexModel : InjectedPageModel
    {
        public IndexModel(IAuthenticationService authenticationService) : base(authenticationService)
        {
        }
        
        public void OnGet()
        {
        }
    }
}