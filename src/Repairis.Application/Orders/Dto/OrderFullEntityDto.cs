using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Repairis.Devices.Dto;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Repairis.Users.Dto;

namespace Repairis.Orders.Dto
{
    public class OrderFullEntityDto : EntityDto<long>
    {
        public DeviceBasicEntityDto Device { get; set; }

        public string AdditionalEquipment { get; set; }

        [Required]
        public string IssueDescription { get; set; }

        public bool IsUrgent { get; set; } = false;

        public bool IsWarrantyComplaint { get; set; } = false;

        [DataType(DataType.MultilineText)]
        public string AdditionalNotes { get; set; }

        public CustomerBasicEntityDto Customer { get; set; }

        //Assigned master
        public long? AssignedEmployeeId { get; set; }

        public virtual EmployeeBasicEntityDto AssignedEmployee { get; set; }

        public OrderStatusEnum OrderStatus { get; set; }

        public bool IsRepaired { get; set; }

        public DateTime? OrderRepairedDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string WorkDoneDescripton { get; set; }

        //Repair price (without spare parts price)
        [DataType(DataType.Currency)]
        public decimal? RepairPrice { get; set; }

        //public virtual List<IssueOrderMapping> IssueOrderMappings { get; set; } = new List<IssueOrderMapping>();

        //Spare parts used to repair the device
        public virtual List<SparePartOrderMapping> SparePartsUsed { get; set; } = new List<SparePartOrderMapping>();

        public bool IsActive { get; set; } = true;

        //Date when device is picked up after repair. Used as warranty start time
        public DateTime? DevicePickupDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
