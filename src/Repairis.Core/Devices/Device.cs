using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.DeviceModels;
using Repairis.Orders;

namespace Repairis.Devices
{
    public class Device : Entity, IPassivable, IDeletionAudited
    {
        [Required]
        [ForeignKey(nameof(DeviceModel))]
        public int DeviceModelId { get; set; }

        public virtual DeviceModel DeviceModel { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
