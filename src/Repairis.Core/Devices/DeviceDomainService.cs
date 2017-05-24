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
        }

        public async Task<Device> GetOrCreateAsync(string deviceCategoryName, string brandName, string deviceModelName, string serialNumber)
        {
            var deviceModel = await _deviceModelDomainService.GetOrCreateAsync(deviceCategoryName, brandName, deviceModelName);

            var device = await _deviceRepository.FirstOrDefaultAsync(x =>
                x.DeviceModelId == deviceModel.Id &&
                x.SerialNumber.ToUpper() == serialNumber.ToUpper());
            
            return device ?? await _deviceRepository.InsertAsync(new Device
            {
                DeviceModel = deviceModel,
                DeviceModelId = deviceModel.Id,
                SerialNumber = serialNumber
            });          
        }
    }
}
