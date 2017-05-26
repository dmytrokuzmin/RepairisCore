namespace Repairis.Users.Dto
{
    public class EmployeeDropDownListDto
    {
        public long Id { get; set; }

        public string FullName
        {
            get
            {
                return $"{Surname} {Name}" + (string.IsNullOrEmpty(FatherName) ? "" : $" {FatherName}");
            }
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
    }
}
