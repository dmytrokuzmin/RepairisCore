using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.DeviceModels.Dto;

namespace Repairis.DeviceModels
{
    public interface IDeviceModelAppService : IApplicationService
    {
        Task CreateDeviceModelAsync(DeviceModelBasicEntityDto input);
        Task<bool> ExistsAsync(DeviceModelBasicEntityDto input);
        Task<DeviceModelViewBagDto> GenerateViewBagDtoAsync();
        Task<DeviceModel> FindDeviceModelAsync(DeviceModelBasicEntityDto input);
        Task<DeviceModelFullEntityDto> GetDeviceModelAsync(int id);
        Task<DeviceModel> GetOrCreateAsync(DeviceModelBasicEntityDto input);
        Task<DeviceModelBasicListDto> GetAllDeviceModelsAsync();
        Task<DeviceModelBasicListDto> GetAllActiveDeviceModelsAsync();
        Task<DeviceModelBasicListDto> GetAllPassiveDeviceModelsAsync();
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
