using Repairis.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace Repairis.Web.Host.Controllers
{
    public class AntiForgeryController : RepairisControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}