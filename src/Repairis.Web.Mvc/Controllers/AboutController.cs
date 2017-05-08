using Repairis.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Repairis.Web.Controllers
{
    public class AboutController : RepairisControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}