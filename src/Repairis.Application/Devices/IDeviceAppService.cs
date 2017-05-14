using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.Devices.Dto;

namespace Repairis.Devices
{
    public interface IDeviceAppService : IApplicationService
    {
        Task<Device> GetOrCreateDeviceAsync(DeviceInput input);
    }
}
