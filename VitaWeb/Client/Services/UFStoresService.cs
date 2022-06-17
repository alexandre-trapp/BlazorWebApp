using Blazored.LocalStorage;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    public class UFStoresService<T> : IUFStoresService<T> where T : class
    {
        public HttpClient _httpClient { get; }
        private readonly ILocalStorageService _localStorageService;

        public UFStoresService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;

            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MFKSERVER_API_URI"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MFKServer");

            _httpClient = httpClient;
        }

        public async Task<bool> DeleteAsync(string Id)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"mfkuf/{Id}");
            requestMessage.Headers.Add("tenant", await _localStorageService.GetItemAsStringAsync("tenantId"));

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            await _httpClient.SendAsync(requestMessage);
            return await Task.FromResult(true);
        }

        public async Task<List<T>> GetAllAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "mfkuf/listar");
            requestMessage.Headers.Add("tenant", await _localStorageService.GetItemAsStringAsync("tenantId"));

            var token = await _localStorageService.GetItemAsStringAsync("accessToken");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return await Task.FromResult(JsonConvert.DeserializeObject<List<T>>(responseBody));
            }
            else
                return null;
        }

        public async Task<T> GetByIdAsync(string Id)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"mfkuf/{Id}");
            requestMessage.Headers.Add("tenant", await _localStorageService.GetItemAsStringAsync("tenantId"));

            var token = await _localStorageService.GetItemAsStringAsync("accessToken");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseBody = await response.Content.ReadAsStringAsync();
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(responseBody));
        }

        public async Task<ResponseBase> SaveAsync(T obj)
        {
            string serializedUser = JsonConvert.SerializeObject(obj);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "mfkuf");
            requestMessage.Headers.Add("tenant", await _localStorageService.GetItemAsStringAsync("tenantId"));

            var token = await _localStorageService.GetItemAsStringAsync("accessToken");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var responseBase = JsonConvert.DeserializeObject<ResponseBase>(responseBody);
                responseBase.StatusCode = response.StatusCode;

                return responseBase;
            }

            return new ResponseBase
            {
                StatusCode = response.StatusCode,
                Message = "UF criada com sucesso."
            };
        }

        public async Task<ResponseBase> UpdateAsync(T obj)
        {
            string serializedUser = JsonConvert.SerializeObject(obj);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, "mfkuf");
            requestMessage.Headers.Add("tenant", await _localStorageService.GetItemAsStringAsync("tenantId"));

            var token = await _localStorageService.GetItemAsync<string>("accessToken");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var responseBase = JsonConvert.DeserializeObject<ResponseBase>(responseBody);
                responseBase.StatusCode = response.StatusCode;

                return responseBase;
            }

            return new ResponseBase
            {
                StatusCode = response.StatusCode,
                Message = "UF atualizada com sucesso."
            };
        }
    }
}
