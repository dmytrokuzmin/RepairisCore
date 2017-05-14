﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repairis.SpareParts.Dto
{
    public class SparePartInput
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
    }
}
