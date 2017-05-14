using Abp.Application.Services.Dto;
using Repairis.Authorization.Users;

namespace Repairis.Users.Dto
{
    public class CustomerBasicEntityDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AdditionalInfo { get; set; }
        public CustomerType CustomerType { get; set; }

    }
}
