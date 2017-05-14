using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Repairis.DeviceModels.Dto
{
    public class DeviceModelBasicEntityDto : EntityDto
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
    }
}
