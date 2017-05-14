using System.Threading.Tasks;
using Repairis.Devices.Dto;

namespace Repairis.Devices
{
    public class DeviceAppService : RepairisAppServiceBase, IDeviceAppService
    {
        private readonly IDeviceDomainService _deviceDomainService;

        public DeviceAppService(IDeviceDomainService deviceDomainService)
        {
            _deviceDomainService = deviceDomainService;
        }

        public async Task<Device> GetOrCreateDeviceAsync(DeviceInput input)
        {
            return await _deviceDomainService.GetOrCreateAsync(input.DeviceCategoryName,
                input.BrandName, input.DeviceModelName, input.SerialNumber);
        }
    }
}
