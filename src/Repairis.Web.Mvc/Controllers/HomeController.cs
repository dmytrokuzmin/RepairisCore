using Abp.AspNetCore.Mvc.Authorization;
using Repairis.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : RepairisControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}