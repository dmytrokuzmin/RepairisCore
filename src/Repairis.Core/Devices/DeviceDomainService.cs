using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Repairis.DeviceModels;

namespace Repairis.Devices
{
    public class DeviceDomainService : DomainService, IDeviceDomainService
    {
        private readonly IDeviceModelDomainService _deviceModelDomainService;
        private readonly IRepository<Device> _deviceRepository;

        public DeviceDomainService(IDeviceModelDomainService deviceModelDomainService, IRepository<Device> deviceRepository)
        {
            _deviceModelDomainService = deviceModelDomainService;
            _deviceRepository = deviceRepository;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        public async Task<Device> GetOrCreateAsync(string deviceCategoryName, string brandName, string deviceModelName, string serialNumber)
        {
            var deviceModel =
                await _deviceModelDomainService.GetOrCreateAsync(deviceCategoryName, brandName, deviceModelName);

            Device device = null;

            if (!string.IsNullOrWhiteSpace(serialNumber))
            {
                device = await _deviceRepository.FirstOrDefaultAsync(x =>
                    x.DeviceModelId == deviceModel.Id &&
                    x.SerialNumber.ToUpper() == serialNumber.ToUpper());
            }

            if (device == null)
            {
                device = new Device
                {
                    DeviceModel = deviceModel,
                    DeviceModelId = deviceModel.Id,
                    SerialNumber = serialNumber
                };
            }

            var id = await _deviceRepository.InsertAndGetIdAsync(device);
            return _deviceRepository.Get(id);
        }
    }
}
