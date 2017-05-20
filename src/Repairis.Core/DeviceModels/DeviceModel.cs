using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.Devices;
using Repairis.SpareParts;

namespace Repairis.DeviceModels
{
    public class DeviceModel : Entity, IPassivable, IDeletionAudited
    {
        [Required]
        [StringLength(RepairisConsts.MaxEntityNameLength)]
        public string DeviceModelName { get; set; }

        [Required]
        [ForeignKey(nameof(DeviceCategory))]
        public int DeviceCategoryId { get; set; }


        public virtual DeviceCategory DeviceCategory { get; set; }

        [Required]
        [ForeignKey(nameof(Brand))]
        public int BrandId { get; set; }


        public virtual Brand Brand { get; set; }

        public virtual List<SparePartCompatibility> CompatibleSpareParts { get; set; } = new List<SparePartCompatibility>();

        public virtual List<Device> Devices { get; set; } = new List<Device>();

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public long? DeleterUserId { get; set; }

        public DateTime? DeletionTime { get; set; }
    }
}
