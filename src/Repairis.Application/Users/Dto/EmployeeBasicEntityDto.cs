using Abp.Application.Services.Dto;

namespace Repairis.Users.Dto
{
    public class EmployeeBasicEntityDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool SalaryIsFlat { get; set; }
        public decimal SalaryValue { get; set; }

    }
}
