using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;

namespace Repairis.DeviceCategories.Dto
{
    [AutoMap(typeof(DeviceCategory))]
    public class DeviceCategoryBasicEntityDto : EntityDto
    {
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.Name")]
        public string DeviceCategoryName { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; }
    }
}
