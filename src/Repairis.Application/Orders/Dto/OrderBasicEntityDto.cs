using System;
using Abp.Application.Services.Dto;
using Repairis.Helpers;

namespace Repairis.Orders.Dto
{
    public class OrderBasicEntityDto : EntityDto<long>
    {   
        public bool IsUrgent { get; set; }
        public bool IsWarrantyComplaint { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }

        public string OrderStatusString
        {
            get { return OrderStatus.GetDisplayName(); }
        }

        public DateTime OrderCreationDate { get; set; }
        public string AssignedMasterFullName { get; set; }
        public string DeviceModel { get; set; }
        public string IssueDescription { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmailAddress { get; set; }

    }
}
