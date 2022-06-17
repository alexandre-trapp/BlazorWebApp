using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    public interface ILicencasService
    {
        public Task<ResponseBase> ExistsLicencesByEmailAsync(string email);
    }
}
