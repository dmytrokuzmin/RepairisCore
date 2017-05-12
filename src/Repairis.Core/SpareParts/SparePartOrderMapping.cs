using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repairis.Orders;

namespace Repairis.SpareParts
{
    public class SparePartOrderMapping
    {
        // All keys are configured via Fluent API

        public long OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int SparePartId { get; set; }
        public virtual SparePart SparePart { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [DataType(DataType.Currency)]
        public decimal PricePerItem { get; set; }
    }
}
