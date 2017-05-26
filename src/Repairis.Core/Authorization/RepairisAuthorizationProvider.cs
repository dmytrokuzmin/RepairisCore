using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using Repairis.Authorization.Roles;

namespace Repairis.Authorization
{
    public class RepairisAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages) ?? context.CreatePermission(PermissionNames.Pages, L("Pages"));

            var users = pages.CreateChildPermission(PermissionNames.Pages_Users, L("Users"));
            
            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var orders = pages.CreateChildPermission(PermissionNames.Pages_Orders, L("Orders"));
            var customers = pages.CreateChildPermission(PermissionNames.Pages_Customers, L("Customers"));
            var brands = pages.CreateChildPermission(PermissionNames.Pages_Brands, L("Brands"));
            var deviceCategories = pages.CreateChildPermission(PermissionNames.Pages_DeviceCategories, L("DeviceCategories"));
            var deviceModels = pages.CreateChildPermission(PermissionNames.Pages_DeviceModels, L("DeviceModels"));
            var devices = pages.CreateChildPermission(PermissionNames.Pages_Devices, L("Devices"));
            var spareParts = pages.CreateChildPermission(PermissionNames.Pages_SpareParts, L("SpareParts"));
            var reports = pages.CreateChildPermission(PermissionNames.Pages_Reports, L("Reports"));
            var employees = pages.CreateChildPermission(PermissionNames.Pages_Employees, L("Employees"));
            var dashboard = pages.CreateChildPermission(PermissionNames.Pages_Dashboard, L("Dashboard"));

            //Customer permissions
            var customerPages = context.GetPermissionOrNull(PermissionNames.CustomerPages) ?? context.CreatePermission(PermissionNames.CustomerPages, L("CustomerPages"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RepairisConsts.LocalizationSourceName);
        }
    }
}
