using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Repairis.Authorization;
using Repairis.Authorization.Users;
using Repairis.Users.Dto;
using Microsoft.AspNetCore.Identity;

namespace Repairis.Users
{
    /* THIS IS JUST A SAMPLE. */
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : RepairisAppServiceBase, IUserAppService
    {
        private readonly IRepository<User, long> _userRepository;
        private readonly IPermissionManager _permissionManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserAppService(
            IRepository<User, long> userRepository, 
            IPermissionManager permissionManager,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _permissionManager = permissionManager;
            _passwordHasher = passwordHasher;
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


        public async Task<User> GetOrCreateCustomerAsync(CustomerInput input)
        {
            var customer = await _userRepository.FirstOrDefaultAsync(
                x => x.Name.ToUpper() == input.Name.ToUpper() &&
                     x.Surname.ToUpper() == input.Surname.ToUpper() &&
                     x.FatherName.ToUpper() == input.FatherName.ToUpper() &&
                     (x.EmailAddress.ToUpper() == input.EmailAddress.ToUpper() ||
                      x.PhoneNumber == input.PhoneNumber));

            if (customer == null)
            {
                customer = await CreateCustomerAsync(input);

                //var customerInfo = (new CustomerInfo
                //{
                //    CustomerType = input.CustomerType,
                //    AdditionalInfo = input.AdditionalInfo,
                //    User = customer
                //});

                //customer.CustomerInfo = customerInfo;

                //var customerRole = await _roleManager.GetRoleByNameAsync("Customer");
                //customer.Roles.Add(new UserRole(null, customer.Id, customerRole.Id));          
            }

            return customer;
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
                throw new UserFriendlyException(LocalizationSource.GetString("CustomerNotFound"));
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


        public async Task<User> CreateCustomerAsync(CustomerInput input)
        {
            string password = User.CreateRandomPassword();

            var user = new User
            {
                Name = input.Name,
                Surname = input.Surname,
                FatherName = input.FatherName,
                PhoneNumber = input.PhoneNumber,
                SecondaryPhoneNumber = input.SecondaryPhoneNumber,
                EmailAddress = input.EmailAddress,
                Address = input.Address,
                UserName = input.EmailAddress,
                IsEmailConfirmed = false,
                IsActive = true,
                IsDeleted = false,
                IsPhoneNumberConfirmed = false,
                CustomerInfo = new CustomerInfo
                {
                    CustomerType = input.CustomerType,
                    AdditionalInfo = input.AdditionalInfo,
                }
            };
            user.Password = _passwordHasher.HashPassword(user, password);
            var customerId = await _userRepository.InsertAndGetIdAsync(user
            );

            return await _userRepository.GetAsync(customerId);
        }


        public async Task<ListResultDto<EmployeeBasicEntityDto>> GetAllActiveEmployeesAsync()
        {
            //UserManager.GetRolesAsync()
            //var role = _roleManager.GetRoleByNameAsync("Employee");
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
                throw new UserFriendlyException(LocalizationSource.GetString("EmployeeNotFound"));
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


        public async Task<User> CreateEmployeeAsync(EmployeeInput input)
        {
            string password = User.CreateRandomPassword();
            var user = new User
            {
                Name = input.Name,
                Surname = input.Surname,
                FatherName = input.FatherName,
                PhoneNumber = input.PhoneNumber,
                SecondaryPhoneNumber = input.SecondaryPhoneNumber,
                EmailAddress = input.EmailAddress,
                Address = input.Address,
                UserName = input.EmailAddress,
                IsEmailConfirmed = false,
                IsActive = true,
                IsDeleted = false,
                IsPhoneNumberConfirmed = true,
                EmployeeInfo = new EmployeeInfo
                {
                    SalaryIsFlat = input.SalaryIsFlat,
                    SalaryValue = input.SalaryValue
                }
            };
            user.Password = _passwordHasher.HashPassword(user, password);
            var employeeId = await _userRepository.InsertAndGetIdAsync(user);

            return await _userRepository.GetAsync(employeeId);
        }
    }
}