using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.Orders;

namespace Repairis.Authorization.Users
{
    [Table("CustomerInfo")]
    public class CustomerInfo : Entity<long>, IDeletionAudited
    {
        [Required]
        public long CustomerUserId { get; set; }

        [ForeignKey(nameof(CustomerUserId))]
        public virtual User CustomerUser { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(2048)]
        public string AdditionalInfo { get; set; }

        public CustomerType CustomerType { get; set; } = CustomerType.Default;

        public bool IsDeleted { get; set; } = false;

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual List<Order> CustomerOrders { get; set; } = new List<Order>();
    }

    public enum CustomerType
    {
        Default = 0,
        Vip = 1,
        BlackListed = 2
    }
}
