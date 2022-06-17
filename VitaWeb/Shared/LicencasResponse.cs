using System.Net;

namespace VitaWeb.Shared
{
    public class LicencasResponseBase
    {
        public IEnumerable<LicencaResponse> Licencas { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }

    public class LicencaResponse
    {
        public string? Solucao { get; set; }
        public int? NumeroLicenca { get; set; }
        public string? CnpjCpf { get; set; }
        public string? NomeFantasia { get; set; }
    }
}
