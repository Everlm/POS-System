using Microsoft.VisualBasic;
using POS.API.Authentication;
using POS.Utilities.Static;

namespace POS.API.CustomAttribute
{
    //API KEY PARA PARA FILTROS DE MINIMAL API
    //public class ApiKeyEndpointFilter : IEndpointFilter
    //{
    //    private readonly IApiKeyValidation _apiKeyValidation;

    //    public ApiKeyEndpointFilter(IApiKeyValidation apiKeyValidation)
    //    {
    //        _apiKeyValidation = apiKeyValidation;
    //    }

    //    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    //    {
    //        if (string.IsNullOrWhiteSpace(context.HttpContext.Request.Headers[ApiKeySetting.ApiKeyHeaderName].ToString()))
    //            return Results.BadRequest();

    //        string? apiKey = context.HttpContext.Request.Headers[ApiKeySetting.ApiKeyHeaderName];

    //        if (!_apiKeyValidation.IsValidApiKey(apiKey!))
    //        {
    //            return Results.Unauthorized();
    //        }

    //        return await next(context);
    //    }
    //}
}
