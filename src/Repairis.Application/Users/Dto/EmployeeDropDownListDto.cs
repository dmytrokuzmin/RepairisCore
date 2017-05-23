using Abp.Application.Services.Dto;

namespace Repairis.Users.Dto
{
    public class EmployeeDropDownListDto : EntityDto<long>
    {
        public string FullName
        {
            get
            {
                return $"{Surname} {Name[0]}." + (string.IsNullOrEmpty(FatherName) ? "" : $" {FatherName[0]}.");
            }
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
    }
}
