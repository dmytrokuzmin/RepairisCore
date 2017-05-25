using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Repairis.Authorization.Users;

namespace Repairis.Users.Dto
{
    public class EmployeeInput
    {
        [Required]
        [StringLength(32)]
        public string Surname { get; set; }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [StringLength(40)]
        public string FatherName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        public string SecondaryPhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }


        [DataType(DataType.MultilineText)]
        [StringLength(550)]
        public string Address { get; set; }

        public bool SalaryIsFlat { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal SalaryValue { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
    }
}
