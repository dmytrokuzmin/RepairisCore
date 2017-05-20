using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repairis.Controllers;
using Repairis.DeviceModels;
using Repairis.DeviceModels.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DeviceModelsController : RepairisControllerBase
    {
        private readonly IDeviceModelAppService _deviceModelAppService;
        public DeviceModelsController(IDeviceModelAppService deviceModelAppService)
        {
            _deviceModelAppService = deviceModelAppService;
        }


        // GET: DeviceModels
        public async Task<ActionResult> Index()
        {
            var deviceModels = await _deviceModelAppService.GetAllDeviceModelsAsync();
            return View(deviceModels);
        }


        // GET: DeviceModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var deviceModel = await _deviceModelAppService.GetDeviceModelAsync((int)id);
            return View(deviceModel);
        }


        // GET: DeviceModels/Create
        public ActionResult Create()
        {       
            return View();
        }


        // POST: DeviceModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Post(DeviceModelBasicEntityDto input)
        {
            if (ModelState.IsValid)
            {
                await _deviceModelAppService.CreateDeviceModelAsync(input);
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: DeviceModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var deviceModel = await _deviceModelAppService.GetDeviceModelAsync((int)id);
            return View(deviceModel);
        }


        // POST: DeviceModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _deviceModelAppService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
