using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.DeviceCategories.Dto;

namespace Repairis.DeviceCategories
{
    public interface IDeviceCategoryAppService : IApplicationService
    {
        Task CreateDeviceCategoryAsync(DeviceCategoryBasicEntityDto input);

        Task<DeviceCategoryBasicListDto> GetAllDeviceCategoriesAsync();

        Task<DeviceCategoryBasicListDto> GetAllActiveDeviceCategoriesAsync();

        Task<DeviceCategoryBasicListDto> GetAllPassiveDeviceCategoriesAsync();
        Task<DeviceCategory> GetOrCreateAsync(string deviceCategoryName);

        Task<DeviceCategoryFullEntityDto> GetDeviceCategoryAsync(int id);

        Task<DeviceCategoryFullEntityDto> GetDeviceCategoryAsync(string deviceCategoryName);

        Task DeleteAsync(int id);

        Task RestoreAsync(int id);
    }
}
