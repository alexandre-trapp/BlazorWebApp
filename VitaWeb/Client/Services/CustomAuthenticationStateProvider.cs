using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net;
using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorageService { get; }
        private readonly IAuthenticationService _authenticationService;        

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, 
            IAuthenticationService authenticationService)
        {
            _localStorageService = localStorageService;
            _authenticationService = authenticationService;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {   
            var accessToken = await _localStorageService.GetItemAsStringAsync("accessToken");           
            
            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(accessToken) && !"null".Equals(accessToken))
            {
                UsuarioAutenticadoResponse user = await _authenticationService.GetUserByAccessTokenAsync(accessToken);
                if (user == null || user.StatusCode != HttpStatusCode.OK)
                    identity = new ClaimsIdentity();
                else 
                    identity = GetClaimsIdentity(user);
            }
            else
                identity = new ClaimsIdentity();

            var claimsPrincipal = new ClaimsPrincipal(identity);            
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(UsuarioAutenticadoResponse user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.Token);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("refreshToken");
            await _localStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(UsuarioAutenticadoResponse user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Email != null)
            { 
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim(ClaimTypes.Name, user.Email)
                                }, "apiauth_type");
            }

            return claimsIdentity;
        }
    }
}
