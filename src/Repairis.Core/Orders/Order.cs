using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.Authorization.Users;
using Repairis.Devices;
using Repairis.Issues;
using Repairis.SpareParts;

namespace Repairis.Orders
{
    public class Order : FullAuditedEntity<long>, IPassivable
    {
        [Required]
        [ForeignKey(nameof(Device))]
        public int DeviceId { get; set; }

        public virtual Device Device { get; set; }

        public string AdditionalEquipment { get; set; }

        [Required]
        public string IssueDescription { get; set; }

        public bool IsUrgent { get; set; } = false;

        public bool IsWarrantyComplaint { get; set; } = false;

        public string AdditionalNotes { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public long CustomerId { get; set; }
        public virtual CustomerInfo Customer { get; set; }

        //Assigned master
        [ForeignKey(nameof(AssignedEmployee))]
        public long? AssignedEmployeeId { get; set; }

        public virtual EmployeeInfo AssignedEmployee { get; set; }

        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Open;

        public DateTime? OrderRepairedDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string WorkDoneDescripton { get; set; }

        //Repair price (doesn't include spare parts price)
        [DataType(DataType.Currency)]
        public decimal? RepairPrice { get; set; }

        public decimal? OrderTotalPrice
        {
            get { return RepairPrice.GetValueOrDefault(0m) + SparePartsUsed.Sum(x => x.Quantity * x.PricePerItem); }
        }

        public virtual List<IssueOrderMapping> Issues { get; set; } = new List<IssueOrderMapping>();

        //Spare parts used to repair the device
        public virtual List<SparePartOrderMapping> SparePartsUsed { get; set; } = new List<SparePartOrderMapping>();

        public bool IsActive { get; set; } = true;

        //Date when device is picked up after repair. Used as warranty start time
        public DateTime? DevicePickupDate { get; set; }

        public DateTime? WarrantyExpirationDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public enum OrderStatusEnum
    {
        [Display(Name = "Open")]
        Open = 0,
        [Display(Name = "Waiting")]
        Waiting = 1,
        [Display(Name = "In progress")]
        InProgress = 2,
        [Display(Name = "Ready")]
        Ready = 4,
        [Display(Name = "On warranty")]
        OnWarranty = 5,
        [Display(Name = "Archived")]
        Closed = 6
    }
}
