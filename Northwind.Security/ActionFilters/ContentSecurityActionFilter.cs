using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Security.ActionFilters
{
    // also use this in meta for page = <meta http-equiv="Content-Security-Policy" content="...">

    /// <summary>
    /// Sets the content security policy to each response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ContentSecurityActionFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting([NotNull]ResultExecutingContext context)
        {
            if (context.Result is ViewResult || context.Result is PageResult)
            {
                // TODO would be nice to have this in config (only if it will not cause any slowdown)
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
                string csp = "default-src 'self'; " +
                    "object-src 'none'; " +
                    "font-src 'self'; " +
                    "img-src 'self' http://www.w3.org data:; " +
                    "connect-src 'self'; " +
                    "frame-src 'none'; " +
                    "manifest-src 'self'; " +
                    "media-src 'self'; " +
                    "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                    "prefetch-src 'self'; " +
                    "style-src 'self' 'unsafe-inline'; " +
                    "frame-ancestors 'none'; " +
                    "sandbox allow-forms allow-same-origin allow-scripts; " +
                    "base-uri 'self'; " +
                    "block-all-mixed-content; " +
                    "upgrade-insecure-requests; " +
                    "child-src 'none'";

                // once for standards compliant browsers
                if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
                }

                // and once again for IE
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy            
                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", "no-referrer");
                }
                
                // https://cheatsheetseries.owasp.org/cheatsheets/DotNet_Security_Cheat_Sheet.html
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Permitted-Cross-Domain-Policies"))
                {
                    context.HttpContext.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "master-only");
                }

                if (!context.HttpContext.Response.Headers.ContainsKey("X-XSS-Protection"))
                {
                    context.HttpContext.Response.Headers.Add("X-XSS-Protection", "0");
                }
            }
            
            base.OnResultExecuting(context);
        }
    }
}
