using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Services
{
    public interface IProviderService
    {
        Task<List<Provider>> GetProviders();
        Task<Provider> GetProviderById(int id);
        Task<int> CreateProvider(string providerName);
    }
}
