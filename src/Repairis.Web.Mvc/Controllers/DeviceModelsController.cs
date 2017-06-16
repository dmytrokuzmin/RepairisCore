using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Authorization;
using Repairis.Controllers;
using Repairis.DeviceModels;
using Repairis.DeviceModels.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_DeviceModels)]
    public class DeviceModelsController : RepairisControllerBase
    {
        private readonly IDeviceModelAppService _deviceModelAppService;
        private readonly IRepository<DeviceModel> _deviceModelRepository;
        public DeviceModelsController(IDeviceModelAppService deviceModelAppService, IRepository<DeviceModel> deviceModelRepository)
        {
            _deviceModelAppService = deviceModelAppService;
            _deviceModelRepository = deviceModelRepository;
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
                try
                {
                    await _deviceModelAppService.CreateDeviceModelAsync(input);
                    return RedirectToAction("Index");
                }
                catch (UserFriendlyException ex)
                {
                    ModelState.AddModelError(nameof(input.DeviceModelName), ex.Message);
                }
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


        [Route("/api/DeviceModels/")]
        [DontWrapResult]
        public ActionResult DeviceModelsDataSource()
        {
            IQueryable<DeviceModelBasicEntityDto> deviceModelsQueryable = _deviceModelRepository.GetAll().ProjectTo<DeviceModelBasicEntityDto>();
            var brandName = Request.Headers["brandName"];
            var deviceCategoryName = Request.Headers["deviceCategoryName"];
            bool allHeadersFilled = false;

            if (!string.IsNullOrEmpty(brandName) && !string.IsNullOrEmpty(brandName[0])
                && !string.IsNullOrEmpty(deviceCategoryName) && !string.IsNullOrEmpty(deviceCategoryName[0]))
            {
                deviceModelsQueryable = deviceModelsQueryable.Where(x => x.BrandName.ToUpper() == brandName[0].ToUpper()
                && x.DeviceCategoryName.ToUpper() == deviceCategoryName[0].ToUpper());
                allHeadersFilled = true;
            }


            return Json(allHeadersFilled ? deviceModelsQueryable.ToDynamicList() : new List<dynamic>(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }
    }
}
