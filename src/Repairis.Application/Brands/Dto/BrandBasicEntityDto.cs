using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Repairis.Brands.Dto
{
    [AutoMap(typeof(Brand))]
    public class BrandBasicEntityDto : EntityDto
    {
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [Required]
        public string BrandName { get; set; }

        public bool IsActive { get; set; }
    }
}
