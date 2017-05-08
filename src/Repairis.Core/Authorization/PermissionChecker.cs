using Abp.Authorization;
using Repairis.Authorization.Roles;
using Repairis.Authorization.Users;
using Repairis.MultiTenancy;

namespace Repairis.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
