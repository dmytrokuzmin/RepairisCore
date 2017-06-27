using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repairis.DeviceModels.Dto;

namespace Repairis.Web.Helpers
{
    public interface IFormHelper : ISingletonDependency
    {
        Task<List<SelectListItem>> GetBrands(bool includeDisaled = false);

        Task<List<SelectListItem>> GetDeviceCategories(bool includeDisaled = false);

        Task<List<DeviceModelAutocompleteDto>> GetDeviceModels();
    }
}
