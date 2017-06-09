using System.ComponentModel.DataAnnotations;
using Abp.Localization;
using Abp.Runtime.Validation;

namespace Repairis.SpareParts.Dto
{
    public class SparePartInput
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.Name")]
        public string SparePartName { get; set; }

        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Code")]
        public string SparePartCode { get; set; }

        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Field.Input.BrandName")]
        public string BrandName { get; set; }

        [StringLength(2048)]
        [DataType(DataType.MultilineText)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Notes")]
        public string Notes { get; set; }

        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SupplierPrice")]
        public decimal SupplierPrice { get; set; }

        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "RecommendedPrice")]
        public decimal? RecommendedPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "StockQuantity")]
        public int StockQuantity { get; set; } = 0;

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Status")]
        public StockStatusEnum StockStatus { get; set; } = StockStatusEnum.OutOfStock;
    }
}
