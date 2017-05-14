using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Repairis.DeviceModels.Dto;
using Repairis.SpareParts.Dto;

namespace Repairis.Brands.Dto
{
    [AutoMapFrom(typeof(Brand))]
    public class BrandFullEntityDto : EntityDto
    {
        public string BrandName { get; set; }
        public bool IsActive { get; set; }
        public List<DeviceModelBasicEntityDto> DeviceModels { get; set; }
        public List<SparePartBasicEntityDto> SpareParts { get; set; }
    }
}
