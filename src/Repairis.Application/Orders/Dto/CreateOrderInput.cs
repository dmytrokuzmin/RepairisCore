using System.ComponentModel.DataAnnotations;
using Abp.Localization;
using Repairis.Devices.Dto;
using Repairis.Users.Dto;

namespace Repairis.Orders.Dto
{
    public class CreateOrderInput
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsUrgent")]
        public bool IsUrgent { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IsWarrantyComplaint")]
        public bool IsWarrantyComplaint { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Device")]
        public DeviceInput Device { get; set; }

        [StringLength(1024)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalEquipment")]
        public string AdditionalEquipment { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "IssueDescription")]
        public string IssueDescription { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Customer")]
        public CustomerInput Customer { get; set; }

        [StringLength(2048)]
        [DataType(DataType.MultilineText)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalNotes")]
        public string AdditionalNotes { get; set; }
    }
}
