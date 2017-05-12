using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace Repairis.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        [StringLength(MaxNameLength)]
        public string FatherName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        public override string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(26)]
        [Phone]
        public string SecondaryPhoneNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(550)]
        public string Address { get; set; }

        [ForeignKey(nameof(EmployeeInfoId))]
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public long? EmployeeInfoId { get; set; }


        [ForeignKey(nameof(CustomerInfoId))]
        public virtual CustomerInfo CustomerInfo { get; set; }
        public long? CustomerInfoId { get; set; }


        [NotMapped]
        public override string FullName
        {
            get
            {
                return string.IsNullOrEmpty(FatherName) ? $"{Surname} {Name}" : $"{Surname} {Name} {FatherName}";
            }
        }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}