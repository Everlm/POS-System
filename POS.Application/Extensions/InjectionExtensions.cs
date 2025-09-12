using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Application.Commons.Filters;
using POS.Application.Commons.Ordering;
using POS.Application.Documents;
using POS.Application.Extensions.WatchDog;
using POS.Application.Interfaces;
using POS.Application.Services;
using POS.Infrastructure.FileStorage;
using POS.Utilities.Static;
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
            services.AddScoped<IDocumentGenerator, DocumentGenerator>();
            services.AddScoped<IDocumentFactory, DocumentFactory>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<ICategoryApplication, CategoryApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IProviderApplication, ProviderApplication>();
            services.AddScoped<IDocumentTypeApplication, DocumentTypeApplication>();
            services.AddScoped<IWarehouseApplication, WarehouseApplication>();
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddScoped<IProductStockApplication, ProductStockApplication>();
            services.AddScoped<IPurcharseApplication, PurcharseApplication>();
            services.AddScoped<IClientApplication, ClientApplication>();
            services.AddScoped<ISaleApplication, SaleApplication>();
            services.AddScoped<IVoucherDoumentTypeApplication, VoucherDoumentTypeApplication>();
            services.AddScoped<IAuthApplication, AuthApplication>();

            services.AddScoped<IGenerateExcelApplication, GenerateExcelApplication>();
            services.AddTransient<IOrderingQuery, OrderingQuery>();
            services.AddTransient<IFilterService, FilterService>();
            services.AddTransient<IAzureStorage, AzureStorage>();
            services.AddTransient<IFileLocalStorageApplication, FileLocalStorageApplication>();
            services.AddWatchDog();

            //API Refit Clients
            services.AddMyRefitClient<ICategoryApiRefit>(ApiNames.POSApi);

            return services;
        }
    }
}
