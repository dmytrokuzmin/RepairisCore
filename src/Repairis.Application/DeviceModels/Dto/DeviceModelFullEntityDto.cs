using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Repairis.Devices;
using Repairis.SpareParts;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelFullEntityDto : EntityDto
    {
        public string DeviceModelName { get; set; }
        public string BrandName { get; set; }
        public string DeviceCategoryName { get; set; }
        public bool IsActive { get; set; }
        public List<SparePartCompatibility> CompatibleSpareParts { get; set; }
        public List<Device> Devices { get; set; }
    }
}
