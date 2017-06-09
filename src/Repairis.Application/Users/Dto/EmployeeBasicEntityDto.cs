using Abp.Application.Services.Dto;
using Abp.Localization;

namespace Repairis.Users.Dto
{
    public class EmployeeBasicEntityDto : EntityDto<long>
    {
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
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SalaryIsFlat")]
        public bool SalaryIsFlat { get; set; }
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SalaryValue")]
        public decimal SalaryValue { get; set; }

    }
}
