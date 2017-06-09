using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Localization;

namespace Repairis.Brands.Dto
{
    [AutoMap(typeof(Brand))]
    public class BrandBasicEntityDto : EntityDto
    {
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.Name")]
        public string BrandName { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; }
    }
}
