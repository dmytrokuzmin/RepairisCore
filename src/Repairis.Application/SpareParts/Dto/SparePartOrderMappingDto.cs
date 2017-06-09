using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Abp.Localization;

namespace Repairis.SpareParts.Dto
{
    [AutoMap(typeof(SparePartOrderMapping))]
    public class SparePartOrderMappingDto
    {
        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Order")]
        public int OrderId { get; set; }

        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SparePart")]
        public int SparePartId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Quantity")]
        public int Quantity { get; set; } = 1;

        [Required]
        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "PricePerItem")]
        public decimal PricePerItem { get; set; }
    }
}
