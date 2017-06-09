using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization;
using Repairis.Authorization.Users;

namespace Repairis.Users.Dto
{
    public class CustomerInput
    {
        [Required]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "CustomerType")]
        public CustomerType CustomerType { get; set; } = CustomerType.Default;

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

        [DataType(DataType.MultilineText)]
        [StringLength(2048)]
        [AbpDisplayName(RepairisConsts.LocalizationSourceName, "AdditionalInfo")]
        public string AdditionalInfo { get; set; }
    }
}
