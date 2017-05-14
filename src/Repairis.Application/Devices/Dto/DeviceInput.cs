using System.ComponentModel.DataAnnotations;

namespace Repairis.Devices.Dto
{
    public class DeviceInput
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string DeviceModelName { get; set; }

        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string BrandName { get; set; }

        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string DeviceCategoryName { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }
    }
}
