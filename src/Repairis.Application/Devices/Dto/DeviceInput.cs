using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace Repairis.Devices.Dto
{
    public class DeviceInput
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

        [StringLength(100)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SerialNumber")]
        public string SerialNumber { get; set; }
    }
}
