using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Data.Entities.User;
using PasswordManager.Services.TokenService;

namespace PasswordManager.Web.Middleware
{
    public class JWTAuthorizer
    {
        private readonly RequestDelegate _next;

        private readonly IDataAccessService _dataAccessService;

        public JWTAuthorizer(RequestDelegate next, IDataAccessService dataAccessService)
        {
            _next = next;
            _dataAccessService = dataAccessService;
        }
        
        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? context.Request.Cookies["Authorization"]?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                await _injectUserInContext(context, tokenService, token);
            }

            await _next(context);
        }

        private async Task _injectUserInContext(HttpContext context, ITokenService tokenService, string token)
        {
            try
            {
                var tokenValid = tokenService.ValidateToken(token);
        
                if (!tokenValid)
                {
                    // delete cookie to ensure this won't happen again
                    context.Response.Cookies.Delete("Authorization");
                    return;
                };
        
                var jwtToken = new JwtSecurityToken(token);
        
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
        
                context.Items.Add("User", await _dataAccessService.Get<UserEntity>(userId));
            }
            catch
            {
                // there's no user to inject
                // delete cookie to ensure this won't happen again
                context.Response.Cookies.Delete("Authorization");
            }
        }
    }
}