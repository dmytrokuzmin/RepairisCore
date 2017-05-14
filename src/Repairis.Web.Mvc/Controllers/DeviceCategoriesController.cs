using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repairis.Controllers;
using Repairis.DeviceCategories;
using Repairis.DeviceCategories.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DeviceCategoriesController : RepairisControllerBase
    {
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;

        public DeviceCategoriesController(IDeviceCategoryAppService deviceCategoryAppService)
        {
            _deviceCategoryAppService = deviceCategoryAppService;
        }
        // GET: DeviceCategories
        public async Task<ActionResult> Index()
        {
            var categories = await _deviceCategoryAppService.GetAllActiveDeviceCategoriesAsync();
            return View(categories);
        }

        // GET: DeviceCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var deviceCategory = await _deviceCategoryAppService.GetDeviceCategoryAsync((int)id);

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
                await _deviceCategoryAppService.CreateDeviceCategoryAsync(deviceCategory);
                return RedirectToAction("Index");
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
