using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using POS.Utilities.Static;
using Refit;

namespace POS.Application.Extensions;

public static class RefitClientExtension
{
    public static IHttpClientBuilder AddMyRefitClient<T>(
        this IServiceCollection services,
        string apiName) where T : class
    {
        return services
            .AddRefitClient<T>()
            .ConfigureHttpClient((sp, c) =>
            {
                var allApis = sp.GetRequiredService<IOptions<ApiServicesSettings>>().Value;

                if (!allApis.TryGetValue(apiName, out var apiUrl) || string.IsNullOrWhiteSpace(apiUrl))
                {
                    throw new InvalidOperationException($"La URL para '{apiName}' no está configurada en 'APIServices'.");
                }

                c.BaseAddress = new Uri(apiUrl);
            });
    }
}


