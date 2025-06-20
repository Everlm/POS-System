using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using POS.Api.Extensions;
using POS.API.Authentication;
using POS.API.CustomAttribute;
using POS.Utilities.AppSettings;
using System.Security.Claims;
using System.Text;

namespace POS.API.Extensions
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Adds and configures authentication services to the specified <see cref="IServiceCollection"/>.
        /// This includes JWT Bearer authentication with token validation parameters,
        /// configuration for Google settings, and registration of API key validation services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add authentication services to.</param>
        /// <param name="configuration">The application configuration containing authentication settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with authentication services registered.</returns>
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = ClaimTypes.Role,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/authHub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.Configure<AppSettings>(configuration.GetSection("GoogleSettings"));
            services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
            services.AddScoped<ApiKeyAuthFilter>();


            return services;
        }
    }
}
