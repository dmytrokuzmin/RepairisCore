using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;
using Repairis.Brands;
using Repairis.Controllers;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SparePartsController : RepairisControllerBase
    {
        private readonly IRepository<SparePart> _sparePartRepository;
        private readonly ISparePartAppService _sparePartAppService;
        private readonly IBrandAppService _brandAppService;


        public SparePartsController(IRepository<SparePart> sparePartRepository, ISparePartAppService sparePartAppService, IBrandAppService brandAppService)
        {
            _sparePartRepository = sparePartRepository;
            _sparePartAppService = sparePartAppService;
            _brandAppService = brandAppService;
        }

        // GET: SpareParts
        public async Task<ActionResult> Index()
        {
            var spareParts = await _sparePartAppService.GetSparePartsAsync();
            return View(spareParts);
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
        public async Task<ActionResult> Edit(SparePartFullEntityDto input)
        {
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
                //sparePart.CompatibleDeviceModels = input.CompatibleDeviceModels;

                await _sparePartRepository.InsertOrUpdateAsync(sparePart);

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
