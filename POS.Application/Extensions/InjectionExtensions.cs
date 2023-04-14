using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Interfaces;
using POS.Application.Interfaces.WatchDog;
using POS.Application.Services;
using POS.Infrastructure.FileStorage;
using System.Reflection;

namespace POS.Application.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ICategoryApplication, CategoryApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IProviderApplication, ProviderApplication>();
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IClientApplication, ClientApplication>();
            services.AddScoped<ISaleApplication, SaleApplication>();
            services.AddScoped<IPurchaseApplication, PurchaseApplication>();
            services.AddScoped<IDepartmentApplication, DepartmentApplication>();
            services.AddScoped<IProvinceApplication, ProvinceApplication>();
            services.AddScoped<IDistrictApplication, DistrictApplication>();
            services.AddScoped<IBusinessApplication, BusinessApplication>();
            services.AddScoped<IBranchOfficeApplication, BranchOfficeIdApplication>();

            services.AddTransient<IAzureStorage, AzureStorage>();
            services.AddWatchDog();

            return services;
        }
    }
}
