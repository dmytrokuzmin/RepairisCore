using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Repairis.Web.Helpers
{
    public interface IFormHelper : ISingletonDependency
    {
        Task<List<SelectListItem>> GetBrands();

        Task<List<SelectListItem>> GetDeviceCategories();
    }
}
