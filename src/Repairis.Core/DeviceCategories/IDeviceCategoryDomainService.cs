using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Repairis.DeviceCategories
{
    public interface IDeviceCategoryDomainService : IDomainService
    {
        Task<bool> ExistsAsync(string deviceCategoryName);
        Task<DeviceCategory> CreateAsync(DeviceCategory deviceCategory);
        Task<DeviceCategory> CreateAsync(string deviceCategoryName);
        Task<DeviceCategory> GetOrCreateAsync(string deviceCategoryName);
        Task<DeviceCategory> GetByNameAsync(string deviceCategoryName);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
