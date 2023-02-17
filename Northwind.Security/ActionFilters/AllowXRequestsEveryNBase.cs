using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Northwind.Security.Models;
using Patterns.Extensions;

namespace Northwind.Security.ActionFilters
{
    internal class AllowXRequestsEveryNBase
    {
        /// <summary>
        /// Gets or sets a unique name for this Throttle.
        /// </summary>
        /// <remarks>
        /// We'll be inserting a Cache record based on this name and client IP, e.g. "Name-192.168.0.1".
        /// </remarks>
        public string? Name { get; set; }

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
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the content name (themed and from SiteContent) to show upon throttling.  If this is present, the Message parameter will not be used.
        /// </summary>
        public string ThrottledRoute { get; set; } = "/Identity/AccessDenied";

        /// <summary>
        /// The Result needs to be applied to the context of the caller if it is bnot null.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IActionResult? DoWork(FilterContext context)
        {
            IActionResult? result = default;

            // You MUST put this in startup services for this to work.
            // services.AddDistributedMemoryCache();
            IDistributedCache cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>() ?? throw new InvalidOperationException("A distributed memory cache must be available for AllowXRequestsEveryNSecondsActionFilter.");

            string clientAddress = GetIp(context.HttpContext.Request);

            string key = string.Concat("AllowRequests-", Name, "-", clientAddress);
            bool allowExecute = false;
            byte[] currentCacheValue = cache.Get(key);

            if (currentCacheValue == null)
            {
                DateTime expiry = DateTime.Now.AddSeconds(Seconds);

                cache.Set(
                    key,
                    System.Text.UTF32Encoding.Unicode.GetBytes(new RequestRecordModel()
                    {
                        Counter = 1,
                        Expires = expiry
                    }.ToJson())
                    ,
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpiration = expiry
                    });

                allowExecute = true;
            }
            else
            {
                RequestRecordModel value = System.Text.UTF32Encoding.Unicode.GetString(currentCacheValue).JsonConvert<RequestRecordModel>();

                value.Counter++;

                cache.Remove(key);

                // Update the record.
                cache.Set(
                   key,
                   System.Text.UTF32Encoding.Unicode.GetBytes(new RequestRecordModel()
                   {
                       Counter = value.Counter,
                       Expires = value.Expires
                   }.ToJson()),
                   new DistributedCacheEntryOptions()
                   {
                       AbsoluteExpiration = value.Expires
                   });

                allowExecute = value.Counter <= Requests;
            }

            if (!allowExecute)
            {
                if (String.IsNullOrEmpty(Message))
                {
                    Message = "Request Limit Exceeded!";
                }

                if (!string.IsNullOrEmpty(ThrottledRoute))
                {
                    //use SiteContent
                    result = new RedirectToRouteResult(ThrottledRoute);
                }
                else
                {
                    //just send a message (not themed)
                    result = new StatusCodeResult(StatusCodes.Status429TooManyRequests);
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            }

            return result;
        }

        /// <summary>
        /// Attempt to get the ip address of the request from the headers otherwise returns host.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetIp(HttpRequest request)
        {
            StringValues result = string.Empty;

            // x-original-forwarded-for cloudfront
            if (!request.Headers.TryGetValue("X-Real-Ip", out result)) // some nat setups
            {
                if (!request.Headers.TryGetValue("X-Original-Forwarded-For", out result)) // cloudflare
                {
                    if (!request.Headers.TryGetValue("X-Forwarded-For", out result)) // standard
                    {
                        if (!request.Headers.TryGetValue("X-Original-For", out result))
                        {
                            if (!request.Headers.TryGetValue("HTTP_X_FORWARDED_FOR", out result))
                            {
                                if (!request.Headers.TryGetValue("REMOTE_ADDR", out result))
                                {
                                    // fallback value
                                    result = request.Host.Value;
                                }
                            }
                        }
                    }
                }                
            }
            
            

            return result.ToString();
        }
    }
}
