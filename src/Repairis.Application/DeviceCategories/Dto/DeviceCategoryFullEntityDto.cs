using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;
using Repairis.DeviceModels.Dto;

namespace Repairis.DeviceCategories.Dto
{
    [AutoMapFrom(typeof(DeviceCategory))]
    public class DeviceCategoryFullEntityDto : EntityDto
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.Name")]
        public string DeviceCategoryName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "DeviceModels")]
        public List<DeviceModelBasicEntityDto> DeviceModels { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; }
    }
}
