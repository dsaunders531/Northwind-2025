using System.Net;

namespace Patterns
{
    /// <summary>
    /// Data object for the content of a typical error response from a .net rest api.
    /// </summary>
    public class HttpErrorResponse
    {
        public HttpErrorResponse()
        {
            Type = string.Empty;
            Title = string.Empty;
            TraceId = string.Empty;
            Errors = new Dictionary<string, string[]>();
        }

        public string Type { get; set; }
        public string Title { get; set; }
        public HttpStatusCode Status { get; set; }
        public string TraceId { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }    
}
