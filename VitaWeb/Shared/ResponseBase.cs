using System.Net;

namespace VitaWeb.Shared
{
    public class ResponseBase
    {
        public HttpStatusCode StatusCode { get; set; } 
        public string Message { get; set; }
    }
}
