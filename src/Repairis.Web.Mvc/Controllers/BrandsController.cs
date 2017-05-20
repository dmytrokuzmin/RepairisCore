using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repairis.Brands;
using Repairis.Brands.Dto;
using Repairis.Controllers;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class BrandsController : RepairisControllerBase
    {
        private readonly IBrandAppService _brandAppService;

        public BrandsController(IBrandAppService brandAppService)
        {
            _brandAppService = brandAppService;
        }

        // GET: Brands
        public async Task<ActionResult> Index()
        {
            var brands = await _brandAppService.GetAllBrandsAsync();
            return View(brands);
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var brand = await _brandAppService.GetBrandAsync(id.Value);
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BrandBasicEntityDto brand)
        {
            if (ModelState.IsValid)
            {
                await _brandAppService.CreateBrandAsync(brand);
                return RedirectToAction("Index");
            }

            return View(brand);
        }


        // GET: Brands/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var brand = await _brandAppService.GetBrandAsync((int)id);
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _brandAppService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
