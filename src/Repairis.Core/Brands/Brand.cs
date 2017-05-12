using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.DeviceModels;
using Repairis.SpareParts;

namespace Repairis.Brands
{
    public class Brand : Entity, IPassivable, IDeletionAudited
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string BrandName { get; set; }

        public virtual List<DeviceModel> DeviceModels { get; set; } = new List<DeviceModel>();

        public virtual List<SparePart> SpareParts { get; set; } = new List<SparePart>();

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}