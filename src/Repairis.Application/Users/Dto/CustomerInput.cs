using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repairis.Authorization.Users;

namespace Repairis.Users.Dto
{
    public class CustomerInput
    {
        [Required]
        public CustomerType CustomerType { get; set; } = CustomerType.Default;

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

        [DataType(DataType.MultilineText)]
        [StringLength(2048)]
        public string AdditionalInfo { get; set; }
    }
}
