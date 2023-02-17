using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Northwind.Security.ActionFilters
{
    /// <summary>
    /// Restrict the quantity of requests to an endpoint ie: login, password reset, create user.
    /// OWASP recommendation.
    /// </summary>
    /// <remarks>
    /// See https://github.com/OWASP/CheatSheetSeries/blob/master/cheatsheets/DotNet_Security_Cheat_Sheet.md
    /// And See
    /// https://github.com/johnstaveley/SecurityEssentials/blob/master/SecurityEssentials/Core/Attributes/AllowXRequestsEveryXSeconds.cs
    /// Credit John Staveley.
    /// Converted to .NetCore.
    /// Requires DistributedMemoryCache.
    /// Targets Razor Pages.
    /// Does not work on MVC controllers actions.
    /// </remarks>
    public class AllowXRequestsEveryNSecondsPageAttribute : ResultFilterAttribute
    {
        private AllowXRequestsEveryNBase Base { get; set; }

        /// <summary>
        /// Create an instance of the AllowXRequestsEveryNSecondsPageAttribute.
        /// </summary>
        /// <param name="uniqueName">A unique name for the throttle</param>
        /// <param name="requests">Sets the number of requests to allow per client in the given number of seconds.</param>
        /// <param name="seconds">Sets the number of seconds clients must wait before executing this decorated route again.</param>
        /// <param name="throttledRoute">(optional) Sets the content name (themed and from SiteContent) to show upon throttling.  If this is present, the Message parameter will not be used. '/Identity/Account/AccessDenied' if you want a page.</param>
        /// <param name="message">Sets a text message (not themed) that will be sent to the client upon throttling.</param>
        public AllowXRequestsEveryNSecondsPageAttribute(string uniqueName, int requests, int seconds, string throttledRoute = "", string message = "")
        {
            Base = new AllowXRequestsEveryNBase()
            {
                Message = message,
                Name = uniqueName,
                Requests = requests,
                Seconds = seconds,
                ThrottledRoute = throttledRoute
            };
        }

        public override void OnResultExecuting(ResultExecutingContext context) // FilterContext
        {
            IActionResult? result = Base.DoWork(context);

            if (result != default)
            {
                context.Result = result;
            }
        }
    }
}
