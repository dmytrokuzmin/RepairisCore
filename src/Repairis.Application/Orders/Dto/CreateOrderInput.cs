using System.ComponentModel.DataAnnotations;
using Repairis.Devices.Dto;
using Repairis.Users.Dto;

namespace Repairis.Orders.Dto
{
    public class CreateOrderInput
    {
        public bool IsUrgent { get; set; }

        public bool IsWarrantyComplaint { get; set; }

        public DeviceInput Device { get; set; }

        [StringLength(1024)]
        public string AdditionalEquipment { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string IssueDescription { get; set; }

        public CustomerInput Customer { get; set; }

        [StringLength(2048)]
        [DataType(DataType.MultilineText)]
        public string AdditionalNotes { get; set; }
    }
}
