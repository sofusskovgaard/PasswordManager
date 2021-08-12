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

namespace PasswordManager.Web.Middleware
{
    public class JWTAuthorizer
    {
        private readonly RequestDelegate _next;
        
        private readonly IConfiguration _configuration;

        private readonly IDataAccessService _dataAccessService;

        public JWTAuthorizer(RequestDelegate next, IConfiguration configuration, IDataAccessService dataAccessService)
        {
            _next = next;
            _configuration = configuration;
            _dataAccessService = dataAccessService;
        }
        
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last() ?? context.Request.Cookies["Authorization"]?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                await _injectUserInContext(context, token);
            }

            await _next(context);
        }

        private async Task _injectUserInContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = _configuration.GetSection("Jwt").GetValue<string>("secret");
                var secretBytes = Encoding.ASCII.GetBytes(secret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

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