using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels;
using Repairis.SpareParts;

namespace Repairis.Web.Helpers
{
    public class FormHelper : IFormHelper
    {
        private readonly IBrandAppService _brandAppService;
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IRepository<DeviceModel> _deviceModelRepository;
        private readonly IRepository<SparePart> _sparePartRepository;

        public FormHelper(IBrandAppService brandAppService, IDeviceCategoryAppService devicecategoryAppService, IRepository<SparePart> sparePartRepository)
        {
            _brandAppService = brandAppService;
            _deviceCategoryAppService = devicecategoryAppService;
            _sparePartRepository = sparePartRepository;
        }

        public async Task<List<SelectListItem>> GetBrands()
        {
            return (await _brandAppService.GetAllBrandsAsync())
                .Select(x => new SelectListItem { Text = x.BrandName, Value = x.BrandName }).ToList();
        }

        public async Task<List<SelectListItem>> GetDeviceCategories()
        {
            return (await _deviceCategoryAppService.GetAllDeviceCategoriesAsync())
                .Select(x => new SelectListItem { Text = x.DeviceCategoryName, Value = x.DeviceCategoryName }).ToList();
        }
    }
}
