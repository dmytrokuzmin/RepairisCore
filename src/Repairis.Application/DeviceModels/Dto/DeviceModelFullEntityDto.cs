using Abp.Application.Services.Dto;
using Repairis.Brands.Dto;
using Repairis.DeviceCategories.Dto;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelFullEntityDto : EntityDto
    {
        public string DeviceModelName { get; set; }
        public BrandBasicEntityDto Brand { get; set; }
        public DeviceCategoryBasicEntityDto DeviceCategory { get; set; }
    }
}
