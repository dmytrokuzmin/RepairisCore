using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Repairis.Authorization.Users;
using Repairis.Devices.Dto;
using Repairis.SpareParts;

namespace Repairis.Orders.Dto
{
    public class OrderCompletionDto
    {
        public long Id { get; set; }

        [Required]
        public DateTime DevicePickupDate { get; set; } = DateTime.Now;
        public DeviceBasicEntityDto Device { get; set; }
        public string CustomerFullName { get; set; }
        public CustomerType CustomerType { get; set; }
        public string AdditionalEquipment { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime OrderRepairedDate { get; set; }
        public bool IsUrgent { get; set; }
        public bool IsWarrantyComplaint { get; set; }

        public string AdditionalNotes { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int WarrantyMonths { get; set; } = 3;

        public string WorkDoneDescripton { get; set; }

        [DataType(DataType.Currency)]
        public decimal RepairPrice { get; set; }

        public virtual List<SparePartOrderMapping> SparePartsUsed { get; set; } = new List<SparePartOrderMapping>();

        public OrderStatusEnum OrderStatus { get; set; }


    }
}
