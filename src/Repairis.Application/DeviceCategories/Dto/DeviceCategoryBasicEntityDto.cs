using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Repairis.DeviceCategories.Dto
{
    [AutoMap(typeof(DeviceCategory))]
    public class DeviceCategoryBasicEntityDto : EntityDto
    {
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [Required]
        public string DeviceCategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}
