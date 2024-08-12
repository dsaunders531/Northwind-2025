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
    /// Does not work on Razor Pages.
    /// Targets MVC controllers actions.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowXRequestsEveryNSecondsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets a unique name for this Throttle.
        /// </summary>
        /// <remarks>
        /// We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1".
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds clients must wait before executing this decorated route again.
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Gets or sets the number of requests to allow per client in the given number of seconds.
        /// </summary>
        public int Requests { get; set; }

        /// <summary>
        /// Gets or sets a text message (not themed) that will be sent to the client upon throttling.  You can include the token {n} to
        /// show this.Seconds in the message, e.g. "You have performed this action more than {x} times in the last {n} seconds.".
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the content name (themed and from SiteContent) to show upon throttling.  If this is present, the Message parameter will not be used.
        /// </summary>
        public string ThrottledRoute { get; set; } = "/Identity/Account/AccessDenied";

        private AllowXRequestsEveryNBase Base { get; set; }

        public AllowXRequestsEveryNSecondsAttribute() : base()
        {
            Base = new AllowXRequestsEveryNBase()
            {
                Message = Message ?? string.Empty,
                Name = Name ?? string.Empty,
                Requests = Requests,
                Seconds = Seconds,
                ThrottledRoute = ThrottledRoute
            };

            Message = Base.Message;
            Name = Base.Name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IActionResult? result = Base.DoWork(context);

            if (result != default)
            {
                context.Result = result;
            }
        }
    }
}
