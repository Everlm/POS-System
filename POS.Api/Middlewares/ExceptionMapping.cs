using System.Net;

namespace POS.API.Middlewares
{
    public static class ExceptionMapping
    {
        private static readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodeMapping = new()
        {
            { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
            { typeof(InvalidOperationException), HttpStatusCode.BadRequest },
            { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
            { typeof(NotImplementedException), HttpStatusCode.NotImplemented },
            { typeof(TimeoutException), HttpStatusCode.RequestTimeout },
            { typeof(KeyNotFoundException), HttpStatusCode.NotFound },
        };

        public static HttpStatusCode GetStatusCode(Exception ex)
        {
            return _exceptionStatusCodeMapping.TryGetValue(ex.GetType(), out var code)
                ? code
                : HttpStatusCode.InternalServerError;
        }
    }

}
