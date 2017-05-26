using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;

namespace Repairis.SpareParts.Dto
{
    public class SparePartInput : ICustomValidate
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
        public void AddValidationErrors(CustomValidationContext context)
        {
            if (StockQuantity > 0 && StockStatus == StockStatusEnum.OutOfStock)
            {
                context.Results.Add(new ValidationResult("Spare part can not be out of stock if the stock quantity is > 0"));
            }

            if (StockQuantity ==0 && StockStatus == StockStatusEnum.InStock)
            {
                context.Results.Add(new ValidationResult("Spare part can not be in stock if the stock quantity is 0"));
            }
        }
    }
}
