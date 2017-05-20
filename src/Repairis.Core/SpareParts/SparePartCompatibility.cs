using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Repairis.DeviceModels;

namespace Repairis.SpareParts
{
    public class SparePartCompatibility : Entity<string>
    {
        // All keys are configured via Fluent API
        [NotMapped]
        public override string Id
        {
            get { return SparePartId + "-" + DeviceModelId; }
        }

        public SparePart SparePart { get; set; }
        public int SparePartId { get; set; }

        public virtual DeviceModel DeviceModel { get; set; }
        public int DeviceModelId { get; set; }
    }
}
