using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net;
using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    public class LicencasService : ILicencasService
    {
        public HttpClient _httpClient { get; }
        private readonly IMemoryCache _cache;

        public LicencasService(HttpClient httpClient, IMemoryCache cache)
        {
            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MFKSECURITATEM_API_URI")!);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MFKSecuritatem");

            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<ResponseBase> ExistsLicencesByEmailAsync(string email)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"licencas?{email}");
            
            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new ResponseBase
                {
                    StatusCode = response.StatusCode,
                    Message = !string.IsNullOrWhiteSpace(responseBody) ?
                        JsonConvert.DeserializeObject<ResponseBase>(responseBody)!.Message :
                        response.StatusCode.ToString()
                };

            _cache.TryGetValue(email, out IEnumerable<LicencaResponse> licencas);
            if (licencas != null && licencas.Any())
                _cache.Remove(email);

            licencas = JsonConvert.DeserializeObject<IEnumerable<LicencaResponse>>(responseBody)!;
            _cache.Set(email, licencas);

            return new ResponseBase
            {
                StatusCode = response.StatusCode,
                Message = $"Licenças obtidas com sucesso para o email {email} e gravadas no cache."
            };
        }
    }
}
