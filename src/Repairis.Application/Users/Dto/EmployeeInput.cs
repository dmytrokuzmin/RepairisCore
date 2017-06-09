using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Localization;
using Repairis.Authorization.Users;

namespace Repairis.Users.Dto
{
    public class EmployeeInput
    {
        [Required]
        [StringLength(32)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Surname")]
        public string Surname { get; set; }

        [Required]
        [StringLength(32)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Name")]
        public string Name { get; set; }

        [StringLength(40)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "FatherName")]
        public string FatherName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SecondaryPhoneNumber")]
        public string SecondaryPhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Email")]
        public string EmailAddress { get; set; }


        [DataType(DataType.MultilineText)]
        [StringLength(550)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Address")]
        public string Address { get; set; }

        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SalaryIsFlat")]
        public bool SalaryIsFlat { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "SalaryValue")]
        public decimal SalaryValue { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "Password")]
        public string Password { get; set; }
    }
}
