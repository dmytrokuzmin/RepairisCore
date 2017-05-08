using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Repairis.Controllers
{
    public abstract class RepairisControllerBase: AbpController
    {
        protected RepairisControllerBase()
        {
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}