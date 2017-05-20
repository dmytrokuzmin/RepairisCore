using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Repairis.SpareParts.Dto
{
    [AutoMap(typeof(SparePartOrderMapping))]
    public class SparePartOrderMappingDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int SparePartId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [DataType(DataType.Currency)]
        public decimal PricePerItem { get; set; }
    }
}
