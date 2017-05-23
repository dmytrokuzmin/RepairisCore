using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.Orders;

namespace Repairis.Authorization.Users
{
    [Table("EmployeeInfo")]
    public class EmployeeInfo : Entity<long>, IDeletionAudited
    {
        [Required]
        public long EmployeeUserId { get; set; }

        [ForeignKey(nameof(EmployeeUserId))]
        public virtual User EmployeeUser { get; set; }

        public bool SalaryIsFlat { get; set; } = false;

        public decimal SalaryValue { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }

        public virtual List<Order> AssignedOrders { get; set; } = new List<Order>();
    }
}
