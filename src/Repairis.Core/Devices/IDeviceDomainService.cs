using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Repairis.Devices
{
    public interface IDeviceDomainService : IDomainService
    {
        Task<Device> GetOrCreateAsync(string deviceCategoryName, string manufacturerName, string deviceModelName, string serialNumber);
    }
}
