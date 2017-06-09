using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Localization;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelBasicEntityDto : EntityDto
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
    }
}
