using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repairis.Brands.Dto;

namespace Repairis.Brands
{
    public class BrandAppService : RepairisAppServiceBase, IBrandAppService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IBrandDomainService _brandDomainService;

        public BrandAppService(IBrandDomainService brandDomainService, IRepository<Brand> brandRepository)
        {
            _brandDomainService = brandDomainService;
            _brandRepository = brandRepository;
        }


        public async Task CreateBrandAsync(BrandBasicEntityDto input)
        {
            await _brandDomainService.CreateAsync(input.MapTo<Brand>());
        }


        public async Task<BrandFullEntityDto> GetBrandAsync(int id)
        {
            var brand = await _brandRepository.GetAll().Where(x => x.Id == id).ProjectTo<BrandFullEntityDto>().FirstOrDefaultAsync();
            if (brand == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("BrandNotFound"));
            }
            return brand;
        }

        //public async Task<BrandFullEntityDto> GetBrandAsync(string brandName)
        //{
        //    var brand = await _brandDomainService.GetByNameAsync(brandName);
        //    if (brand == null)
        //    {
        //        throw new UserFriendlyException(LocalizationSource.GetString("BrandNotFound"));
        //    }
        //    return brand.MapTo<BrandFullEntityDto>();
        //}


        public async Task<List<BrandBasicEntityDto>> GetAllBrandsAsync()
        {
            return (await _brandRepository.GetAllListAsync()).MapTo<List<BrandBasicEntityDto>>();       
        }

        public Task<Brand> GetOrCreateAsync(string brandName)
        {
            return _brandDomainService.GetOrCreateAsync(brandName);
        }


        public async Task DeleteAsync(int id)
        {
            await _brandDomainService.DeleteAsync(id);
        }

        public async Task RestoreAsync(int id)
        {
            await _brandDomainService.RestoreAsync(id);
        }
    }
}
