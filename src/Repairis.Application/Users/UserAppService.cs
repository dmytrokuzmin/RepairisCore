using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Repairis.Authorization;
using Repairis.Authorization.Users;
using Repairis.Authorization.Roles;
using Repairis.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Repairis.Users
{
    /* THIS IS JUST A SAMPLE. */
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : RepairisAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CustomerInfo, long> _customerInfoRepository;
        private readonly IRepository<EmployeeInfo, long> _employeeInfoRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserRegistrationManager _userRegistrationManager;

        public UserAppService(
            IRepository<User, long> userRepository, 
            IPermissionManager permissionManager,
            IPasswordHasher<User> passwordHasher, UserRegistrationManager userRegistrationManager, IRepository<CustomerInfo, long> customerInfoRepository, IRepository<EmployeeInfo, long> employeeInfoRepository)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _passwordHasher = passwordHasher;
            _userRegistrationManager = userRegistrationManager;
            _customerInfoRepository = customerInfoRepository;
            _employeeInfoRepository = employeeInfoRepository;
        }

        public async Task ProhibitPermission(ProhibitPermissionInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            var permission = _permissionManager.GetPermission(input.PermissionName);

            await UserManager.ProhibitPermissionAsync(user, permission);
        }

        //Example for primitive method parameters.
        public async Task RemoveFromRole(long userId, string roleName)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            await UserManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<ListResultDto<UserListDto>> GetUsers()
        {
            var users = await _userRepository.GetAllListAsync();

            return new ListResultDto<UserListDto>(
                ObjectMapper.Map<List<UserListDto>>(users)
                );
        }

        public async Task CreateUser(CreateUserInput input)
        {
            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.Password = _passwordHasher.HashPassword(user, input.Password);
            user.IsEmailConfirmed = true;

            await UserManager.CreateAsync(user);
        }

        public async Task<ListResultDto<UserListDto>> GetEmployeesAsync()
        {
            var users = await _userRepository.GetAllListAsync(x => x.EmployeeInfo != null);

            return new ListResultDto<UserListDto>(
                users.MapTo<List<UserListDto>>()
            );
        }


        public async Task<ListResultDto<CustomerBasicEntityDto>> GetAllActiveCustomersAsync()
        {
            var customers = await _userRepository.GetAllListAsync(x => x.IsActive && x.CustomerInfoId != null);

            return new ListResultDto<CustomerBasicEntityDto>(
                customers.MapTo<List<CustomerBasicEntityDto>>()
            );
        }


        public async Task<CustomerInfo> GetOrCreateCustomerAsync(CustomerInput input)
        {
            var user = await _userRepository.GetAllIncluding(x => x.CustomerInfo).Where(
                x => x.Name.ToUpper() == input.Name.ToUpper() &&
                     x.Surname.ToUpper() == input.Surname.ToUpper() &&
                     x.FatherName.ToUpper() == input.FatherName.ToUpper() &&
                     (x.EmailAddress.ToUpper() == input.EmailAddress.ToUpper() ||
                      x.PhoneNumber == input.PhoneNumber)).FirstOrDefaultAsync();

            if (user == null)
            {
                return await CreateCustomerAsync(input);      
            }

            if (user.CustomerInfo == null)
            {
                var customerInfo = await _customerInfoRepository.InsertAsync(new CustomerInfo
                {
                    CustomerUserId = user.Id,
                    CustomerType = input.CustomerType,
                    AdditionalInfo = input.AdditionalInfo
                });

                user.CustomerInfoId = customerInfo.Id;
                await _userRepository.UpdateAsync(user);
            }

            return user.CustomerInfo;
        }

        public async Task<CustomerFullEntityDto> GetCustomerDtoAsync(long id)
        {
            var customer = await _userRepository.GetAsync(id);

            if (customer?.CustomerInfo == null)
            {
                throw new UserFriendlyException(L("CustomerNotFound"));
            }

            return customer.MapTo<CustomerFullEntityDto>();
        }


        public async Task DeleteCustomerAsync(long id)
        {
            var customer = await _userRepository.FirstOrDefaultAsync(id);
            if (customer?.CustomerInfo == null)
            {
                throw new UserFriendlyException(L("CustomerNotFound"));
            }

            if (customer.IsActive)
            {
                customer.IsActive = false;
            }
            else
            {
                await _userRepository.DeleteAsync(customer.Id);
            }
        }


        public async Task<CustomerInfo> CreateCustomerAsync(CustomerInput input)
        {
            string password = User.CreateRandomPassword();

            var user = await _userRegistrationManager.RegisterUserAsync(StaticRoleNames.Tenants.Customer, input.Name, input.Surname,
                input.FatherName, input.PhoneNumber, input.SecondaryPhoneNumber, input.Address, input.EmailAddress,
                input.PhoneNumber, password, false);

            var customerInfo = await _customerInfoRepository.InsertAsync(new CustomerInfo
            {
                CustomerUserId = user.Id,
                CustomerType = input.CustomerType,
                AdditionalInfo = input.AdditionalInfo
            });

            user.CustomerInfoId = customerInfo.Id;
            await _userRepository.UpdateAsync(user);

            //send password via email or sms

            return customerInfo;
        }


        public async Task<ListResultDto<EmployeeBasicEntityDto>> GetAllActiveEmployeesAsync()
        {
            var employees = await _userRepository.GetAllListAsync(x => x.IsActive && x.EmployeeInfo != null);

            return new ListResultDto<EmployeeBasicEntityDto>(
                employees.MapTo<List<EmployeeBasicEntityDto>>()
            );
        }


        public async Task<EmployeeFullEntityDto> GetEmployeeDtoAsync(long id)
        {
            var employee = await _userRepository.GetAsync(id);

            if (employee?.EmployeeInfo == null)
            {
                throw new UserFriendlyException(L("EmployeeNotFound"));
            }

            return employee.MapTo<EmployeeFullEntityDto>();
        }


        public async Task DeleteEmployeeAsync(long id)
        {
            var employee = await _userRepository.FirstOrDefaultAsync(id);
            if (employee?.EmployeeInfo == null)
            {
                throw new UserFriendlyException(L("EmployeeNotFound"));
            }

            if (employee.IsActive)
            {
                employee.IsActive = false;
            }
            else
            {
                await _userRepository.DeleteAsync(employee.Id);
            }
        }


        public async Task<EmployeeInfo> CreateEmployeeAsync(EmployeeInput input)
        {

            var user = await _userRegistrationManager.RegisterUserAsync(StaticRoleNames.Tenants.Employee, input.Name, input.Surname,
                input.FatherName, input.PhoneNumber, input.SecondaryPhoneNumber, input.Address, input.EmailAddress,
                input.PhoneNumber, input.Password, false);

            var employeeInfo = await _employeeInfoRepository.InsertAsync(new EmployeeInfo
            {
                EmployeeUserId = user.Id,
                SalaryIsFlat = input.SalaryIsFlat,
                SalaryValue = input.SalaryValue
            });

            user.EmployeeInfoId = employeeInfo.Id;
            await _userRepository.UpdateAsync(user);

            //send password via email or sms

            return employeeInfo;
        }
    }
}