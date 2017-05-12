using Repairis.DeviceModels;

namespace Repairis.SpareParts
{
    public class SparePartCompatibility
    {
        // All keys are configured via Fluent API

        public SparePart SparePart { get; set; }
        public int SparePartId { get; set; }

        public virtual DeviceModel DeviceModel { get; set; }
        public int DeviceModelId { get; set; }
    }
}
