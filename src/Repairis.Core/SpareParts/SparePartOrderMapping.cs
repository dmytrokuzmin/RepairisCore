using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Repairis.Orders;

namespace Repairis.SpareParts
{
    public class SparePartOrderMapping : Entity<string>
    {
        // All keys are configured via Fluent API
        [NotMapped]
        public override string Id
        {
            get { return OrderId + "-" + SparePartId; }
        }

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
