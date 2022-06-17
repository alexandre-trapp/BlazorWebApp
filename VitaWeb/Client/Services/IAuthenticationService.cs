using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    public interface IAuthenticationService
    {
        public Task<UsuarioAutenticadoResponse> RegisterUserAsync(Usuario Usuario);
        public Task<UsuarioAutenticadoResponse> LoginAsync(UsuarioAuthRequest usuario);
        public Task<UsuarioAutenticadoResponse> RefreshTokenAsync(UsuarioAutenticadoResponse usuarioAutenticado);
        public Task<UsuarioAutenticadoResponse> GetUserByAccessTokenAsync(string accessToken);
    }
}
