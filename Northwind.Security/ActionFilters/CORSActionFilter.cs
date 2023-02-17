using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Northwind.Security.ActionFilters
{
    /// <summary>
    /// Allow a server to send content to an allowed origin.
    /// </summary>
    /// <remarks>See <see cref="https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-7.0"/></remarks>
    [Obsolete("Use the built-in startup AddCors extension")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CORSActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting([NotNull] ResultExecutingContext context)
        {            
            if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                // In these cases - the user is authenticated. An origin must be specified.
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", 
                    $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}");
            }
            else
            {
                // allow every origin (unsafe!)
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            // Access-Control-Allow-Methods: POST, GET, OPTIONS
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, PUT, DELETE, OPTIONS, HEAD");
            //Access-Control-Allow-Headers: X-PINGOTHER, Content-Type
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            //Access-Control-Max-Age: 86400 seconds
            context.HttpContext.Response.Headers.Add("Access-Control-Max-Age", "240");
            //Access-Control-Allow-Credentials: true

            base.OnResultExecuting(context);
        }
    }
}
