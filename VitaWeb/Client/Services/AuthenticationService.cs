using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using VitaWeb.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace VitaWeb.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public HttpClient _httpClient { get; }
        private readonly IMemoryCache _cache;

        public AuthenticationService(HttpClient httpClient, IMemoryCache cache)
        {
            httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("MFKSERVER_API_URI"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MFKServer");

            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<UsuarioAutenticadoResponse> RegisterUserAsync(Usuario usuario)
        {
            string serializedUser = JsonConvert.SerializeObject(usuario);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "mfkuu")
            {
                Content = new StringContent(serializedUser)
            };

            requestMessage.Headers.Add("tenant", _cache.Get<string>("tenantId"));
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new UsuarioAutenticadoResponse
                {
                    StatusCode = response.StatusCode,
                    Message = !string.IsNullOrWhiteSpace(responseBody) ? JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody).Message : response.StatusCode.ToString()
                };

            return await LoginAsync(new UsuarioAuthRequest 
            {
                Email = usuario.Email,
                Password = usuario.Password
            });
        }

        public async Task<UsuarioAutenticadoResponse> LoginAsync(UsuarioAuthRequest usuario)
        {
            string serializedUser = JsonConvert.SerializeObject(usuario);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "auth/email")
            {
                Content = new StringContent(serializedUser)
            };

            if (!_cache.TryGetValue("tenantId", out string tenantId))
                tenantId = _cache.Set("tenantId", "10000");
            
            requestMessage.Headers.Add("tenant", tenantId);

            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new UsuarioAutenticadoResponse
                {
                    StatusCode = response.StatusCode,
                    Message = !string.IsNullOrWhiteSpace(responseBody) ? JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody).Message : response.StatusCode.ToString()
                };
            
            var authenticationResponse = JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody);
            authenticationResponse.Email = usuario.Email;
            authenticationResponse.StatusCode = response.StatusCode;

            return await Task.FromResult(authenticationResponse);
        }

        public async Task<UsuarioAutenticadoResponse> RefreshTokenAsync(UsuarioAutenticadoResponse usuarioAutenticado)
        {
            string serializedUser = JsonConvert.SerializeObject(usuarioAutenticado);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "auth/refreshToken")
            {
                Content = new StringContent(serializedUser)
            };

            requestMessage.Headers.Add("tenant", _cache.Get<string>("tenantId"));
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);
            
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new UsuarioAutenticadoResponse
                {
                    StatusCode = response.StatusCode,
                    Message = !string.IsNullOrWhiteSpace(responseBody) ? JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody).Message : response.StatusCode.ToString()
                };

            var returnedUser = JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody);
            returnedUser.Email = usuarioAutenticado.Email;

            return await Task.FromResult(returnedUser);
        }

        public async Task<UsuarioAutenticadoResponse> GetUserByAccessTokenAsync(string token)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "mfkuu/validateUserByAccessToken");

            string serializedToken = JsonConvert.SerializeObject(new { userAccessToken = token });
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            requestMessage.Content = new StringContent(serializedToken);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            
            requestMessage.Headers.Add("tenant", _cache.Get<string>("tenantId"));

            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                return new UsuarioAutenticadoResponse
                {
                    StatusCode = response.StatusCode,
                    Message = !string.IsNullOrWhiteSpace(responseBody) ? JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody).Message : response.StatusCode.ToString()
                };

            var returnedUser = JsonConvert.DeserializeObject<UsuarioAutenticadoResponse>(responseBody);
            returnedUser.Token = token;
            returnedUser.StatusCode = response.StatusCode;

            return await Task.FromResult(returnedUser);
        }
    }
}
