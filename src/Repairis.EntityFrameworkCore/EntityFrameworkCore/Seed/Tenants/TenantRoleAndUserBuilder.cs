using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Repairis.Authorization;
using Repairis.Authorization.Roles;
using Repairis.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Repairis.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly RepairisDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(RepairisDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin) ??
                             CreateAdminRoleForTenant();

            //Employee role
            var employeeRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Employee);
            if (employeeRole == null)
            {
                CreateEmployeeRoleForTenant();
            }

            //Customer role
            var customerRole = _context.Roles.FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Customer);
            if (customerRole == null)
            {
                CreateCustomerRoleForTenant();
            }

            //admin user

            var adminUser = _context.Users.FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }
            }
        }


        private Role CreateAdminRoleForTenant()
        {
            var adminRole = _context.Roles
                .Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true })
                .Entity;
            _context.SaveChanges();

            var disallowedAdminPermissions = new List<string>
            {
                PermissionNames.CustomerPages
            };

            //Grant all permissions to admin role
            var permissions = PermissionFinder
                .GetAllPermissions(new RepairisAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && !disallowedAdminPermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in permissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    });
            }

            _context.SaveChanges();
            return adminRole;
        }


        private void CreateEmployeeRoleForTenant()
        {
            var employeeRoleForTenant = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Employee, StaticRoleNames.Tenants.Employee) { IsStatic = true }).Entity;
            _context.SaveChanges();

            var disallowedEmployeePermissions = new List<string>
            {
                PermissionNames.Pages_Employees,
                PermissionNames.Pages_Reports,
                PermissionNames.Pages_Users,
                PermissionNames.Pages_Tenants,
                PermissionNames.CustomerPages
            };

            //Grant all permissions to employee role
            var employeePermissions = PermissionFinder
                .GetAllPermissions(new RepairisAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && !disallowedEmployeePermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in employeePermissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = employeeRoleForTenant.Id
                    });
            }

            _context.SaveChanges();
        }

        private void CreateCustomerRoleForTenant()
        {
            var customerRole = _context.Roles.Add(
                new Role(_tenantId, StaticRoleNames.Tenants.Customer, StaticRoleNames.Tenants.Customer)
                {
                    IsStatic = true,
                    IsDefault = true
                }).Entity;
            _context.SaveChanges();

            var allowedCustomerPermissions = new List<string>
            {
                PermissionNames.CustomerPages
            };

            var customerPermissions = PermissionFinder
                .GetAllPermissions(new RepairisAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) && allowedCustomerPermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in customerPermissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = customerRole.Id
                    });
            }
        }    
    }
}
