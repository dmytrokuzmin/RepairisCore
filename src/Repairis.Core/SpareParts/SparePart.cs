using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.Brands;

namespace Repairis.SpareParts
{
    public class SparePart : Entity, IDeletionAudited
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string SparePartName { get; set; }

        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string SparePartCode { get; set; }

        [Required]
        public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public virtual Brand Brand { get; set; }

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

        public virtual List<SparePartCompatibility> CompatibleDeviceModels { get; set; } = new List<SparePartCompatibility>();

        public virtual List<SparePartOrderMapping> SparePartOrders { get; set; } = new List<SparePartOrderMapping>();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
    }

    public enum StockStatusEnum
    {
        [Display(Name = "Out of stock")]
        OutOfStock = 0,
        [Display(Name = "Waiting for arrival")]
        WaitingForArrival = 1,
        [Display(Name = "In stock")]
        InStock = 2
    }
}
