using Microsoft.AspNetCore.Authentication.JwtBearer;
using POS.API.Authentication;
using POS.Utilities.Static;

namespace POS.Api.Extensions
{
    public static class AuthorizationPoliciesExtensions
    {
        public static IServiceCollection AddAppAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.ApiKeyPolicy, policy =>
                {
                    policy.AddAuthenticationSchemes(new[] { JwtBearerDefaults.AuthenticationScheme });
                    policy.Requirements.Add(new ApiKeyRequirement());
                });

                options.AddPolicy(AppPolicies.RequireAdminRole, policy =>
                {
                    policy.RequireRole(AppRoles.Admin);
                });

                options.AddPolicy("CanManageProducts", policy =>
                {
                    policy.RequireRole(AppRoles.Admin); 
                    policy.RequireClaim("permission", AppPermissions.Products_Create);
                    policy.RequireClaim("permission", AppPermissions.Products_Edit);

                });

            });

            return services;
        }
    }
}