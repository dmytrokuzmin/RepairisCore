using Abp.AspNetCore.Mvc.Authorization;
using Repairis.Authorization;
using Repairis.Controllers;
using Repairis.MultiTenancy;
using Microsoft.AspNetCore.Mvc;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : RepairisControllerBase
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantsController(ITenantAppService tenantAppService)
        {
            _tenantAppService = tenantAppService;
        }

        public ActionResult Index()
        {
            var output = _tenantAppService.GetTenants();
            return View(output);
        }
    }
}