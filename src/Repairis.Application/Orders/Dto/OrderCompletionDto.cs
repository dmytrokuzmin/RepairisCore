using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Localization;
using Repairis.Authorization.Users;
using Repairis.Devices.Dto;
using Repairis.SpareParts;

namespace Repairis.Orders.Dto
{
    public class OrderCompletionDto
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "ID")]
        public long Id { get; set; }

        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "DevicePickupDate")]
        public DateTime DevicePickupDate { get; set; } = DateTime.Now;
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Device")]
        public DeviceBasicEntityDto Device { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Customer")]
        public string CustomerFullName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CustomerType")]
        public CustomerType CustomerType { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalEquipment")]
        public string AdditionalEquipment { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CreationTime")]
        public DateTime CreationTime { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "OrderRepairedDate")]
        public DateTime OrderRepairedDate { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsUrgent")]
        public bool IsUrgent { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsWarrantyComplaint")]
        public bool IsWarrantyComplaint { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalNotes")]
        public string AdditionalNotes { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "WarrantyMonths")]
        public int WarrantyMonths { get; set; } = 3;

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "WorkDoneDescription")]
        public string WorkDoneDescripton { get; set; }

        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "RepairPrice")]
        public decimal RepairPrice { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SparePartPrice")]
        public decimal SparePartPrice => SparePartsUsed.Sum(x => x.PricePerItem * x.Quantity);

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "TotalPrice")]
        public decimal TotalPrice => RepairPrice + SparePartPrice;

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SparePartsUsed")]
        public virtual List<SparePartOrderMapping> SparePartsUsed { get; set; } = new List<SparePartOrderMapping>();

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Status")]
        public OrderStatusEnum OrderStatus { get; set; }
    }
}
