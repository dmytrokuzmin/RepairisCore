using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Repairis.Devices.Dto
{
    public class DeviceBasicEntityDto : EntityDto
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
