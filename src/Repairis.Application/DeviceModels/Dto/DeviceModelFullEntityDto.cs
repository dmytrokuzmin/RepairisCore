using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Localization;
using Repairis.Devices;
using Repairis.SpareParts;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelFullEntityDto : EntityDto
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.DeviceModelName")]
        public string DeviceModelName { get; set; }
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.BrandName")]
        public string BrandName { get; set; }
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.DeviceCategoryName")]
        public string DeviceCategoryName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CompatibleSpareParts")]
        public List<SparePartCompatibility> CompatibleSpareParts { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Devices")]
        public List<Device> Devices { get; set; }
    }
}
