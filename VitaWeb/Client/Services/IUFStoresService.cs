using VitaWeb.Shared;

namespace VitaWeb.Client.Services
{
    interface IUFStoresService<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string Id);
        Task<ResponseBase> SaveAsync(T obj);
        Task<ResponseBase> UpdateAsync(T obj);
        Task<bool> DeleteAsync(string Id);
    }
}
