using System;
using Abp.Application.Services.Dto;
using Abp.Localization;
using Repairis.Helpers;

namespace Repairis.Orders.Dto
{
    public class OrderBasicEntityDto : EntityDto<long>
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsUrgent")]
        public bool IsUrgent { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsWarrantyComplaint")]
        public bool IsWarrantyComplaint { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Status")]
        public OrderStatusEnum OrderStatus { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Status")]
        public string OrderStatusString
        {
            get { return OrderStatus.GetDisplayName(); }
        }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Created")]
        public DateTime OrderCreationDate { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AssignedMaster")]
        public string AssignedMasterFullName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "DeviceModel")]
        public string DeviceModel { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SerialNumber")]
        public string DeviceSerialNumber { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IssueDescription")]
        public string IssueDescription { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Customer")]
        public string CustomerFullName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "PhoneNumber")]
        public string CustomerPhoneNumber { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Email")]
        public string CustomerEmailAddress { get; set; }

    }
}
