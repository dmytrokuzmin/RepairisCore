using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Repairis.DeviceModels.Dto;

namespace Repairis.DeviceCategories.Dto
{
    [AutoMapFrom(typeof(DeviceCategory))]
    public class DeviceCategoryFullEntityDto : EntityDto
    {
        public string DeviceCategoryName { get; set; }
        public List<DeviceModelBasicEntityDto> DeviceModels { get; set; }

        public bool IsActive { get; set; }
    }
}
