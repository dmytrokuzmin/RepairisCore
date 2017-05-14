using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Repairis.Authorization.Users;
using Repairis.Users.Dto;

namespace Repairis.Users
{
    public interface IUserAppService : IApplicationService
    {
        Task ProhibitPermission(ProhibitPermissionInput input);

        Task RemoveFromRole(long userId, string roleName);

        Task<ListResultDto<UserListDto>> GetUsers();

        Task CreateUser(CreateUserInput input);

        Task<User> CreateCustomerAsync(CustomerInput input);

        Task<User> GetOrCreateCustomerAsync(CustomerInput input);

        Task<ListResultDto<CustomerBasicEntityDto>> GetAllActiveCustomersAsync();

        Task<CustomerFullEntityDto> GetCustomerDtoAsync(long id);

        Task DeleteCustomerAsync(long id);

        Task<User> CreateEmployeeAsync(EmployeeInput input);

        Task<ListResultDto<EmployeeBasicEntityDto>> GetAllActiveEmployeesAsync();

        Task<EmployeeFullEntityDto> GetEmployeeDtoAsync(long id);

        Task DeleteEmployeeAsync(long id);
    }
}