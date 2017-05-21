using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.DeviceModels.Dto;

namespace Repairis.DeviceModels
{
    public interface IDeviceModelAppService : IApplicationService
    {
        Task CreateDeviceModelAsync(DeviceModelBasicEntityDto input);
        Task<bool> ExistsAsync(DeviceModelBasicEntityDto input);
        Task<DeviceModel> FindDeviceModelAsync(DeviceModelBasicEntityDto input);
        Task<DeviceModelBasicEntityDto> GetDeviceModelAsync(int id);
        Task<DeviceModel> GetOrCreateAsync(DeviceModelBasicEntityDto input);
        Task<List<DeviceModelBasicEntityDto>> GetAllDeviceModelsAsync();
        Task<List<DeviceModelAutocompleteDto>> GetAllDeviceModelsForAutocompleteAsync();

        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
    }
}
