using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Infra
{
    public static class AuthenticationService {

        public static WebApplicationBuilder RegisterAuthentication(this WebApplicationBuilder builder)
        {

            builder.Services.AddAuthentication( CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.LoginPath = Constants.GoogleLoginPath; 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            }).AddGoogle(options => {
                options.ClientId = builder.Configuration.GetSection("AuthCredentials").GetSection("Google").GetValue<string>("GoogleClientId") ?? "";
                options.ClientSecret = builder.Configuration.GetSection("AuthCredentials").GetSection("Google").GetValue<string>("GoogleClientSecret") ?? "";
            });

            return builder;
        }
    }
}

