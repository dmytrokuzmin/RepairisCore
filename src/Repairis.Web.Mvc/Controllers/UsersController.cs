using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Repairis.Authorization;
using Repairis.Controllers;
using Repairis.Users;
using Microsoft.AspNetCore.Mvc;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Users)]
    public class UsersController : RepairisControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _userAppService.GetUsers();
            return View(output);
        }
    }
}