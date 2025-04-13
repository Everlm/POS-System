using Microsoft.AspNetCore.Mvc;

namespace POS.API.CustomAttribute
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthFilter))
        {
        }
    }
}
