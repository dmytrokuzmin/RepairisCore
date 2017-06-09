using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Localization;
using Repairis.Devices.Dto;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Repairis.Users.Dto;

namespace Repairis.Orders.Dto
{
    public class OrderFullEntityDto : EntityDto<long>
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CreationTime")]
        public string CreationTime { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Device")]
        public DeviceBasicEntityDto Device { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalEquipment")]
        public string AdditionalEquipment { get; set; }

        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IssueDescription")]
        public string IssueDescription { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsUrgent")]
        public bool IsUrgent { get; set; } = false;

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsWarrantyComplaint")]
        public bool IsWarrantyComplaint { get; set; } = false;

        [DataType(DataType.MultilineText)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalNotes")]
        public string AdditionalNotes { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Customer")]
        public CustomerBasicEntityDto Customer { get; set; }

        //Assigned master
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AssignedMaster")]
        public long AssignedEmployeeId { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Status")]
        public OrderStatusEnum OrderStatus { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "OrderRepairedDate")]
        public DateTime? OrderRepairedDate { get; set; }

        [DataType(DataType.MultilineText)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "WorkDoneDescription")]
        public string WorkDoneDescripton { get; set; }

        //Repair price (without spare parts price)
        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "RepairPrice")]
        public decimal? RepairPrice { get; set; }

        //public virtual List<IssueOrderMapping> IssueOrderMappings { get; set; } = new List<IssueOrderMapping>();

        //Spare parts used to repair the device
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SparePartsUsed")]
        public virtual List<SparePartOrderMapping> SparePartsUsed { get; set; } = new List<SparePartOrderMapping>();

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsActive")]
        public bool IsActive { get; set; } = true;

        //Date when device is picked up after repair. Used as warranty start time
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "DevicePickupDate")]
        public DateTime? DevicePickupDate { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "WarrantyExpirationDate")]
        public DateTime? WarrantyExpirationDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
