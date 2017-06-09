using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;
using Repairis.DeviceModels.Dto;
using Repairis.SpareParts.Dto;

namespace Repairis.Brands.Dto
{
    [AutoMapFrom(typeof(Brand))]
    public class BrandFullEntityDto : EntityDto
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.Name")]
        public string BrandName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "DeviceModels")]
        public List<DeviceModelBasicEntityDto> DeviceModels { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SpareParts")]
        public List<SparePartBasicEntityDto> SpareParts { get; set; }
    }
}
