using Abp.Application.Navigation;
using Abp.Localization;
using Repairis.Authorization;

namespace Repairis.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class RepairisNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Orders,
                        L("Orders"),
                        url: "Orders",
                        icon: "fa fa-book",
                        requiresAuthentication: true
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Customers,
                        L("Customers"),
                        url: "Customers",
                        icon: "fa fa-users",
                        requiresAuthentication: true
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.Devices,
                            L("Devices"),
                            icon: "fa fa-camera",
                            requiresAuthentication: true
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.Brands,
                                L("Brands"),
                                url: "Brands",
                                icon: "fa fa-camera",
                                requiresAuthentication: true
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.DeviceCategories,
                                L("DeviceCategories"),
                                url: "DeviceCategories",
                                icon: "fa fa-camera",
                                requiresAuthentication: true
                            )
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.DeviceModels,
                                L("DeviceModels"),
                                url: "DeviceModels",
                                icon: "fa fa-camera",
                                requiresAuthentication: true
                            )
                        )
                )
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.SpareParts,
                        L("SpareParts"),
                        url: "SpareParts",
                        icon: "fa fa-wrench",
                        requiresAuthentication: true
                    )
                )
                .AddItem(
                    new MenuItemDefinition(
                            PageNames.Reports,
                            L("Reports"),
                            icon: "fa fa-bar-chart"
                        )
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.CompanySalesReport,
                                L("CompanySalesReport"),
                                url: "Reports/CompanySalesReport",
                                requiresAuthentication: true
                            ))
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.CompanyOrdersReport,
                                L("CompanyOrdersReport"),
                                url: "Reports/CompanyOrdersReport",
                                requiresAuthentication: true
                            ))
                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.EmployeesSalaryReport,
                                L("EmployeesSalaryReport"),
                                url: "Reports/EmployeesSalaryReport",
                                requiresAuthentication: true
                            ))

                )
                .AddItem(
                    new MenuItemDefinition(
                            "Settings",
                            new LocalizableString("Settings", "Repairis"),
                            icon: "fa fa-cogs"
                        )

                        .AddItem(
                            new MenuItemDefinition(
                                PageNames.Tenants,
                                L("Tenants"),
                                url: "Tenants",
                                icon: "fa fa-globe",
                                requiredPermissionName: PermissionNames.Pages_Tenants
                            ))
                         .AddItem(
                            new MenuItemDefinition(
                                PageNames.Employees,
                                L("Employees"),
                                url: "Employees",
                                icon: "fa fa-users",
                                requiresAuthentication: true
                            )
                        )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, RepairisConsts.LocalizationSourceName);
        }
    }
}
