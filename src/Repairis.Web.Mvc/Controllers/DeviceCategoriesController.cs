using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Repairis.Authorization;
using Repairis.Controllers;
using Repairis.DeviceCategories;
using Repairis.DeviceCategories.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_DeviceCategories)]
    public class DeviceCategoriesController : RepairisControllerBase
    {
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IRepository<DeviceCategory> _deviceCategoryRepository;

        public DeviceCategoriesController(IDeviceCategoryAppService deviceCategoryAppService, IRepository<DeviceCategory> deviceCategoryRepository)
        {
            _deviceCategoryAppService = deviceCategoryAppService;
            _deviceCategoryRepository = deviceCategoryRepository;
        }
        // GET: DeviceCategories
        public async Task<ActionResult> Index()
        {
            var categories = await _deviceCategoryAppService.GetAllDeviceCategoriesAsync();
            return View(categories);
        }

        // GET: DeviceCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var deviceCategory = await _deviceCategoryAppService.GetDeviceCategoryAsync((int)id);

            return View(deviceCategory);
        }


        // POST: DeviceCategories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DeviceCategoryFullEntityDto deviceCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _deviceCategoryRepository.GetAsync(deviceCategory.Id);
                    if (category == null)
                    {
                        return NotFound();
                    }

                    category.DeviceCategoryName = deviceCategory.DeviceCategoryName;
                    category.IsActive = deviceCategory.IsActive;

                    return RedirectToAction("Index");
                }
                catch (UserFriendlyException ex)
                {
                    ModelState.AddModelError(nameof(deviceCategory.DeviceCategoryName), ex.Message);
                }
            }

            return View(deviceCategory);
        }



        // GET: DeviceCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeviceCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DeviceCategoryBasicEntityDto deviceCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _deviceCategoryAppService.CreateDeviceCategoryAsync(deviceCategory);
                    return RedirectToAction("Index");
                }
                catch (UserFriendlyException ex)
                {
                    ModelState.AddModelError(nameof(deviceCategory.DeviceCategoryName), ex.Message);
                }
            }

            return View(deviceCategory);
        }


        // GET: DeviceCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var deviceCategory = await _deviceCategoryAppService.GetDeviceCategoryAsync((int)id);
            return View(deviceCategory);
        }


        // POST: DeviceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _deviceCategoryAppService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
