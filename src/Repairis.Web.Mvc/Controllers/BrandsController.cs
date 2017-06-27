using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Repairis.Authorization;
using Repairis.Brands;
using Repairis.Brands.Dto;
using Repairis.Controllers;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Brands)]
    public class BrandsController : RepairisControllerBase
    {
        private readonly IBrandAppService _brandAppService;
        private readonly IRepository<Brand> _brandRepository;

        public BrandsController(IBrandAppService brandAppService, IRepository<Brand> brandRepository)
        {
            _brandAppService = brandAppService;
            _brandRepository = brandRepository;
        }

        // GET: Brands
        public async Task<ActionResult> Index()
        {
            var brands = await _brandAppService.GetAllBrandsAsync();
            return View(brands);
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var brand = await _brandAppService.GetBrandAsync(id.Value);
            return View(brand);
        }

        // POST: DeviceCategories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BrandFullEntityDto input)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var brand = await _brandRepository.GetAsync(input.Id);
                    if (brand == null)
                    {
                        return NotFound();
                    }

                    brand.BrandName = input.BrandName;
                    brand.IsActive = input.IsActive;

                    return RedirectToAction("Index");
                }
                catch (UserFriendlyException ex)
                {
                    ModelState.AddModelError(nameof(input.BrandName), ex.Message);
                }
            }

            return View(input);
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
                try
                {
                    await _brandAppService.CreateBrandAsync(brand);
                    return RedirectToAction("Index");
                }
                catch (UserFriendlyException ex)
                {
                    ModelState.AddModelError(nameof(brand.BrandName), ex.Message);
                }
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
