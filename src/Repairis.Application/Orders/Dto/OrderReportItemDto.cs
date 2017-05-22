using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repairis.Orders.Dto
{
    public class OrderReportItemDto
    {
        public decimal RepairPrice { get; set; }
        public decimal SparePartsTotalCost { get; set; }
        public decimal SparePartsTotalSupplierCost { get; set; }

        public string AssignedMasterFullName { get; set; }

        public DateTime? DevicePickupDate { get; set; }       
    }
}
