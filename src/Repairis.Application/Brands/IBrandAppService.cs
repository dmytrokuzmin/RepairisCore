using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.Brands.Dto;

namespace Repairis.Brands
{
    public interface IBrandAppService: IApplicationService
    {
        Task CreateBrandAsync(BrandBasicEntityDto input);

        Task<List<BrandBasicEntityDto>> GetAllBrandsAsync();

        Task<Brand> GetOrCreateAsync(string brandName);

        Task<BrandFullEntityDto> GetBrandAsync(int id);

        //Task<BrandFullEntityDto> GetBrandAsync(string brandName);

        Task DeleteAsync(int id);

        Task RestoreAsync(int id);
    }
}
