using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Repairis.SpareParts.Dto
{
    public class SparePartFullEntityDto : EntityDto
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string SparePartName { get; set; }

        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string SparePartCode { get; set; }

        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string BrandName { get; set; }

        [StringLength(2048)]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [DataType(DataType.Currency)]
        public decimal? SupplierPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal? RecommendedPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; } = 0;

        public StockStatusEnum StockStatus { get; set; } = StockStatusEnum.OutOfStock;

        public List<int> CompatibleDeviceModelIds { get; set; } = new List<int>();
    }
}
