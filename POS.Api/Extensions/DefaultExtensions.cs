using Microsoft.AspNetCore.Authorization;
using POS.Api.Hubs;
using POS.API.Authentication;
using POS.Application.Interfaces;

namespace POS.API.Extensions;

public static class DefaultExtensions
{
    public static IServiceCollection DefaultServices(this IServiceCollection services, string cors)
    {

      
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
        services.AddHttpContextAccessor();
        services.AddSignalR();

        services.AddCors(options =>
        {
            options.AddPolicy(name: cors,
            builder =>
            {
                builder.WithOrigins(
                    "http://localhost:4200",
                    "https://tuproduccion.com",
                    "https://localhost:443",
                    "http://localhost",
                    "http://localhost:80"
                    )
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
        return services;
    }

    public static IServiceCollection AppScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
        services.AddScoped<ISignalRNotifierService, SignalRNotifierService>();
        return services;
    }
}
