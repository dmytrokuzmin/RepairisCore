using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Validation;
using Abp.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Authorization;
using Repairis.Brands;
using Repairis.Controllers;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_SpareParts)]
    public class SparePartsController : RepairisControllerBase
    {
        private readonly IRepository<SparePart> _sparePartRepository;
        private readonly IRepository<SparePartCompatibility, string> _compatibilityRepository;
        private readonly ISparePartAppService _sparePartAppService;
        private readonly IBrandAppService _brandAppService;


        public SparePartsController(IRepository<SparePart> sparePartRepository, ISparePartAppService sparePartAppService, IBrandAppService brandAppService, IRepository<SparePartCompatibility, string> compatibilityRepository)
        {
            _sparePartRepository = sparePartRepository;
            _sparePartAppService = sparePartAppService;
            _brandAppService = brandAppService;
            _compatibilityRepository = compatibilityRepository;
        }

        // GET: SpareParts
        public async Task<ActionResult> Index()
        {
            var spareParts = await _sparePartAppService.GetSparePartsAsync();
            return View(spareParts);
        }


        [Route("/api/SpareParts/")]
        [DontWrapResult]
        public ActionResult SparePartsDataSource([FromBody] DataManager dm)
        {
            IQueryable<SparePart> sparePartsQueryable;
            var deviceModelId = Request.Headers["deviceModelId"];
            if (string.IsNullOrEmpty(deviceModelId))
            {
                sparePartsQueryable = _sparePartRepository.GetAll();
            }
            else
            {
                sparePartsQueryable = _compatibilityRepository
                    .GetAllIncluding(x => x.SparePart)
                    .Where(x => x.DeviceModelId == int.Parse(deviceModelId[0])).Select(x => x.SparePart);
            }
            int count = sparePartsQueryable.Count();
            IEnumerable data = sparePartsQueryable.ProjectTo<SparePartBasicEntityDto>();
            DataOperations operation = new DataOperations();
            data = operation.Execute(data, dm);

            return Json(new { result = data.ToDynamicList(), count = count }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        // GET: SpareParts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SpareParts/Create
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        public async Task<ActionResult> Create_Post(SparePartInput input)
        {
            if (input.StockStatus == StockStatusEnum.OutOfStock && input.StockQuantity > 0)
            {
                ModelState.AddModelError(nameof(input.StockQuantity), L("OutOfStockRequiresStockQuantityZero"));
            }
            if (input.StockStatus == StockStatusEnum.InStock && input.StockQuantity == 0)
            {
                ModelState.AddModelError(nameof(input.StockQuantity), L("InStockRequiresStockQuantityHigherThanZero"));
            }

            if (ModelState.IsValid)
            {
                await _sparePartAppService.CreateSparePartAsync(input);
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: SpareParts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var sparePartDto = await _sparePartAppService.GetSparePartDtoAsync((int)id);
            if (sparePartDto == null)
            {
                return NotFound();
            }

            return View(sparePartDto);
        }


        // POST: SpareParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        [UnitOfWork]
        public async Task<ActionResult> Edit(SparePartFullEntityDto input)
        {
            if (input.StockStatus == StockStatusEnum.OutOfStock && input.StockQuantity > 0)
            {
                ModelState.AddModelError(nameof(input.StockQuantity), L("OutOfStockRequiresStockQuantityZero"));
            }
            if (input.StockStatus == StockStatusEnum.InStock && input.StockQuantity == 0)
            {
                ModelState.AddModelError(nameof(input.StockQuantity), L("InStockRequiresStockQuantityHigherThanZero"));
            }

            if (ModelState.IsValid)
            {
                var sparePart = await _sparePartRepository.GetAsync(input.Id);
                var brand = await _brandAppService.GetOrCreateAsync(input.BrandName);

                sparePart.Brand = brand;
                sparePart.SparePartName = input.SparePartName;
                sparePart.SparePartCode = input.SparePartCode;
                sparePart.SupplierPrice = input.SupplierPrice;
                sparePart.RecommendedPrice = input.RecommendedPrice;
                sparePart.StockStatus = input.StockStatus;
                sparePart.StockQuantity = input.StockQuantity;
                await _sparePartRepository.InsertOrUpdateAsync(sparePart);

                var inputDeviceModelIds = input.CompatibleDeviceModelIds.Distinct();
                await _compatibilityRepository.DeleteAsync(x => x.SparePartId == sparePart.Id);
                CurrentUnitOfWork.SaveChanges();
                foreach (var deviceModelId in inputDeviceModelIds.Where(x => x.HasValue))
                {
                    await _compatibilityRepository.InsertAsync(new SparePartCompatibility
                    {
                        DeviceModelId = deviceModelId.Value,
                        SparePartId = sparePart.Id
                    });
                    CurrentUnitOfWork.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: SpareParts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SparePart sparePart = await _sparePartRepository.GetAsync((int)id);
            if (sparePart == null)
            {
                return NotFound();
            }
            return View(sparePart.MapTo<SparePartBasicEntityDto>());
        }


        // POST: SpareParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _sparePartRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
