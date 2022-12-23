using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    /// <summary>
    /// Data object for the content of a typical error response from a .net rest api.
    /// </summary>
    public class HttpErrorResponse
    {
        public HttpErrorResponse()
        {
            this.Type = string.Empty;
            this.Title = string.Empty;
            this.TraceId = string.Empty;
            this.Errors = new Dictionary<string, string[]>();
        }

        public string Type { get; set; }
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }    
}
