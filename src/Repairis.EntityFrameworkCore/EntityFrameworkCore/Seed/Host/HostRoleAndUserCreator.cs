using System.Collections.Generic;
using System.Linq;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Repairis.Authorization;
using Repairis.Authorization.Roles;
using Repairis.Authorization.Users;

namespace Repairis.EntityFrameworkCore.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly RepairisDbContext _context;

        public HostRoleAndUserCreator(RepairisDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            //Admin role for host

            var adminRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin) ??
                                   CreateAdminRoleForHost();

            //Employee role for host

            var employeeRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Employee);
            if (employeeRoleForHost == null)
            {
                CreateEmployeeRoleForHost();
            }

            //Customer role for host

            var customerRoleForHost = _context.Roles.FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Customer);
            if (customerRoleForHost == null)
            {
                CreateCustomerRoleForHost();
            }

            //admin user for host

            var adminUserForHost = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "admin@aspnetzero.com",
                    IsEmailConfirmed = true,
                    IsActive = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                };

                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();
            
                //User account of admin user
                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = null,
                    UserId = adminUserForHost.Id,
                    UserName = AbpUserBase.AdminUserName,
                    EmailAddress = adminUserForHost.EmailAddress
                });

                _context.SaveChanges();
            }
        }

        private Role CreateAdminRoleForHost()
        {
            var adminRoleForHost = _context.Roles
                .Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) {IsStatic = true}).Entity;
            _context.SaveChanges();

            var disallowedAdminPermissions = new List<string>
            {
                PermissionNames.CustomerPages
            };

            //Grant all permissions to admin role
            var adminPermissions = PermissionFinder
                .GetAllPermissions(new RepairisAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) && !disallowedAdminPermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in adminPermissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRoleForHost.Id
                    });
            }

            _context.SaveChanges();
            return adminRoleForHost;
        }

     
        private void CreateEmployeeRoleForHost()
        {
            var employeeRoleForHost = _context.Roles
                .Add(new Role(null, StaticRoleNames.Host.Employee, StaticRoleNames.Host.Employee) {IsStatic = true}).Entity;
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
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) && !disallowedEmployeePermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in employeePermissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = employeeRoleForHost.Id
                    });
            }

            _context.SaveChanges();
        }


        private void CreateCustomerRoleForHost()
        {
            var customerRoleForHost = _context.Roles.Add(
                new Role(null, StaticRoleNames.Host.Customer,
                        StaticRoleNames.Host.Customer)
                    { IsStatic = true, IsDefault = true }).Entity;
            _context.SaveChanges();

            var allowedCustomerPermissions = new List<string>
            {
                PermissionNames.CustomerPages
            };

            var customerPermissions = PermissionFinder
                .GetAllPermissions(new RepairisAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Host) && allowedCustomerPermissions.Contains(p.Name))
                .ToList();

            foreach (var permission in customerPermissions)
            {
                _context.Permissions.Add(
                    new RolePermissionSetting
                    {
                        TenantId = null,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = customerRoleForHost.Id
                    });
            }
        }
    }
}