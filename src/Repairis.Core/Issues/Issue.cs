using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.DeviceCategories;

namespace Repairis.Issues
{
    public class Issue : Entity, IPassivable, IDeletionAudited
    {
        [Required]
        public int DeviceCategoryId { get; set; }

        [ForeignKey(nameof(DeviceCategoryId))]
        public virtual DeviceCategory DeviceCategory { get; set; }

        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string IssueName { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }

        public virtual List<IssueOrderMapping> OrdersWithIssue { get; set; } = new List<IssueOrderMapping>();
    }
}
