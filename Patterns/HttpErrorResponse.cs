// <copyright file="HttpErrorResponse.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Net;

namespace Patterns
{
    /// <summary>
    /// Data object for the content of a typical error response from a .net rest api.
    /// </summary>
    public class HttpErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpErrorResponse"/> class.
        /// </summary>
        public HttpErrorResponse()
        {
            Type = string.Empty;
            Title = string.Empty;
            TraceId = string.Empty;
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Gets or sets the type of error response.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the response title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the status code for the response.
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// Gets or sets the request id.
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// Gets or sets a list of errors.
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
