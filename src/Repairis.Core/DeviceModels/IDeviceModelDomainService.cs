using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Repairis.DeviceModels
{
    public interface IDeviceModelDomainService : IDomainService
    {
        Task<bool> ExistsAsync(string deviceCategoryName, string brandName, string deviceModelName);

        Task CreateAsync(DeviceModel deviceModel);
        Task CreateAsync(string deviceCategoryName, string brandName, string deviceModelName);
        Task<int> CreateAndGetIdAsync(string deviceCategoryName, string brandName, string deviceModelName);

        Task<DeviceModel> FindDeviceModelAsync(string deviceCategoryName, string brandName, string deviceModelName);
        Task<DeviceModel> GetOrCreateAsync(string deviceCategoryName, string brandName, string deviceModelName);
        Task<DeviceModel> GetDeviceModelAsync(int id);

        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
