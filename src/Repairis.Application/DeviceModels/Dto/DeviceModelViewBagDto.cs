using System.Collections.Generic;
using Repairis.Brands.Dto;
using Repairis.DeviceCategories.Dto;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelViewBagDto
    {
        public List<BrandBasicEntityDto> Brands { get; set; }
        public List<DeviceCategoryBasicEntityDto> DeviceCategories { get; set; }
        public List<DeviceModelBasicEntityDto> DeviceModels { get; set; }
    }
}
