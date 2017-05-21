using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels;
using Repairis.DeviceModels.Dto;

namespace Repairis.Web.Helpers
{
    public class FormHelper : IFormHelper
    {
        private readonly IBrandAppService _brandAppService;
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IDeviceModelAppService _deviceModelAppService;


        public FormHelper(IBrandAppService brandAppService, IDeviceCategoryAppService devicecategoryAppService, IDeviceModelAppService deviceModelAppService)
        {
            _brandAppService = brandAppService;
            _deviceCategoryAppService = devicecategoryAppService;
            _deviceModelAppService = deviceModelAppService;
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
    }
}
