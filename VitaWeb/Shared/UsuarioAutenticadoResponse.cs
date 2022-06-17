using System;
using System.Net;

namespace VitaWeb.Shared
{
    public class UsuarioAutenticadoResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
