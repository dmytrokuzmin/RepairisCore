using Abp.Application.Services.Dto;
using Abp.Localization;
using Repairis.Authorization.Users;
using Repairis.Helpers;

namespace Repairis.Users.Dto
{
    public class CustomerBasicEntityDto : EntityDto<long>
    {
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CustomerType")]
        public string CustomerTypeString
        {
            get { return CustomerType.GetDisplayName(); }
        }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Name")]
        public string Name { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Surname")]
        public string Surname { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "FatherName")]
        public string FatherName { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Address")]
        public string Address { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SecondaryPhoneNumber")]
        public string SecondaryPhoneNumber { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Email")]
        public string EmailAddress { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalInfo")]
        public string AdditionalInfo { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CustomerType")]
        public CustomerType CustomerType { get; set; }
    }
}
