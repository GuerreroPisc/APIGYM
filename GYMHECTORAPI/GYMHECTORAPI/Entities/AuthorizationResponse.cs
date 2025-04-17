using System.Net;

namespace GYMHECTORAPI.Entities
{
    public class AuthorizationResponse
    {
        public HttpStatusCode Codigo { get; set; }
        public string Mensaje { get; set; }
        public int? IdUsuario { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
