using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PasswordManager.Data.DataAccessService;
using PasswordManager.Services.AuthenticationService;
using PasswordManager.Services.EntityServices.UsersEntityService;
using PasswordManager.Services.PasswordHashingService;
using PasswordManager.Services.TokenService;
using PasswordManager.Web.Middleware;

namespace PasswordManager.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Inject configuration.
            services.AddSingleton<IConfiguration>(Configuration);
            
            // Database service.
            services.AddSingleton<IDataAccessService, DataAccessService>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<JWTAuthorizer>();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}