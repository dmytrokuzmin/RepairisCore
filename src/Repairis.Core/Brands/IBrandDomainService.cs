using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Repairis.Brands
{
    public interface IBrandDomainService : IDomainService
    {
        Task<bool> ExistsAsync(string brandName);
        Task<Brand> CreateAsync(string brandName);
        Task<Brand> CreateAsync(Brand brand);
        Task<Brand> GetOrCreateAsync(string brandName);
        Task<Brand> GetByNameAsync(string name);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
