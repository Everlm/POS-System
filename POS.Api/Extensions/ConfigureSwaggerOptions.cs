using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace POS.API.Extensions;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo
            {
                Title = "POS",
                Version = description.ApiVersion.ToString(),
                Description = "A simple POS API",
                Contact = new OpenApiContact
                {
                    Name = "EverCodes",
                    Email = "Everlm17@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/deveverlm/")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += "<font color=\"#FF0000\"><b>This API version has been DEPRECATED.</b></font>";
            }

            options.SwaggerDoc(description.GroupName, info);
        }
    }
}
