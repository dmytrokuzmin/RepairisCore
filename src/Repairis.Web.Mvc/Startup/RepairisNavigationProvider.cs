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
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fa fa-home",
                        requiresAuthentication: true
                    )
                )
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
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Users,
                                L("Users"),
                                url: "Users",
                                icon: "fa fa-users",
                                requiredPermissionName: PermissionNames.Pages_Users
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.About,
                                L("About"),
                                url: "About",
                                icon: "fa fa-info"
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
