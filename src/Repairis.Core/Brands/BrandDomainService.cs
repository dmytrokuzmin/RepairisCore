using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.UI;

namespace Repairis.Brands
{
    public class BrandDomainService : DomainService, IBrandDomainService
    {
        private readonly IRepository<Brand> _brandRepository;

        public BrandDomainService(IRepository<Brand> brandRepository)
        {
            _brandRepository = brandRepository;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public async Task<Brand> GetByNameAsync(string name)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                return await _brandRepository.FirstOrDefaultAsync(
                    m => m.BrandName.ToUpper() == name.ToUpper());
            }
        }


        public async Task<bool> ExistsAsync(string brandName)
        {
            return await GetByNameAsync(brandName) != null;
        }


        public async Task<Brand> CreateAsync(string brandName)
        {
            return await CreateAsync(new Brand { BrandName = brandName });
        }


        public async Task<Brand> CreateAsync(Brand brand)
        {
            var id = await _brandRepository.InsertAndGetIdAsync(brand);
            return await _brandRepository.GetAsync(id);
        }


        public async Task<Brand> GetOrCreateAsync(string brandName)
        {
            var brand = await GetByNameAsync(brandName);

            if (brand == null)
            {
                brand = new Brand { BrandName = brandName };
                _brandRepository.Insert(brand);
            }

            return brand;

        }


        public async Task DeleteAsync(int id)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(id);

            if (brand == null)
            {
                return;
            }

            if ((brand.DeviceModels.Count != 0) || (brand.SpareParts.Count != 0))
            {
                throw new UserFriendlyException(
                    LocalizationSource.GetString("CannotDeleteBrandBecauseIsBeingUsed"));
            }

            if (brand.IsActive)
            {
                brand.IsActive = false;
            }
            else
            {
                await _brandRepository.DeleteAsync(id);
            }
        }

        public async Task RestoreAsync(int id)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(id);

            if (brand == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("BrandNotFound"));
            }

            brand.IsActive = true;
        }
    }
}
