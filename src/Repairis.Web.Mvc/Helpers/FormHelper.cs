using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels;
using Repairis.DeviceModels.Dto;
using Repairis.Orders;

namespace Repairis.Web.Helpers
{
    public class FormHelper : IFormHelper
    {
        private readonly IBrandAppService _brandAppService;
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IDeviceModelAppService _deviceModelAppService;
        private readonly IRepository<Order,long> _ordersReporitory;



        public FormHelper(IBrandAppService brandAppService, IDeviceCategoryAppService devicecategoryAppService, IDeviceModelAppService deviceModelAppService, IRepository<Order, long> ordersReporitory)
        {
            _brandAppService = brandAppService;
            _deviceCategoryAppService = devicecategoryAppService;
            _deviceModelAppService = deviceModelAppService;
            _ordersReporitory = ordersReporitory;
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

        public async Task<List<DeviceModelAutocompleteDto>> GetDeviceModels()
        {
            return await _deviceModelAppService.GetAllDeviceModelsForAutocompleteAsync();
        }

        public async Task<List<int>> GetYearsForReport()
        {
            var firstOrder = await _ordersReporitory.GetAll().OrderBy(x => x.CreationTime).FirstOrDefaultAsync();
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();

            int firstYear = firstOrder == null ? currentYear : firstOrder.CreationTime.Year;
            while (currentYear >= firstYear)
            {
                years.Add(currentYear);
                currentYear--;
            }

            return years;
        }
    }
}
