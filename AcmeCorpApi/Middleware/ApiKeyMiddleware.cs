using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcmeCorpApi.Middleware
{
    public class ApiKeyMiddleware 
    {
        private readonly RequestDelegate _next;
        private const string APIKEY = "AcmeApiKey";
        public ApiKeyMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context) 
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey)) 
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Please provide an API key!\n");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEY);
            if (!apiKey.Equals(extractedApiKey)) 
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized\n");
                return;
            }
            await _next(context);
        }
    }
}