using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.UI;

namespace Repairis.DeviceCategories
{
    public class DeviceCategoryDomainService : DomainService, IDeviceCategoryDomainService
    {
        private readonly IRepository<DeviceCategory> _deviceCategoryRepository;

        public DeviceCategoryDomainService(IRepository<DeviceCategory> deviceCategoryRepository)
        {
            _deviceCategoryRepository = deviceCategoryRepository;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        [UnitOfWork]
        public async Task<DeviceCategory> GetByNameAsync(string deviceCategoryName)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                return await _deviceCategoryRepository.FirstOrDefaultAsync(
                    category => category.DeviceCategoryName.ToUpper() == deviceCategoryName.ToUpper());
            }
        }


        public async Task<bool> ExistsAsync(string deviceCategoryName)
        {
            return await GetByNameAsync(deviceCategoryName) != null;
        }


        public async Task<DeviceCategory> CreateAsync(string deviceCategoryName)
        {
            return await CreateAsync(new DeviceCategory
            {
                DeviceCategoryName = deviceCategoryName
            });
        }


        public async Task<DeviceCategory> CreateAsync(DeviceCategory deviceCategory)
        {
            var id = await _deviceCategoryRepository.InsertAndGetIdAsync(deviceCategory);
            return await _deviceCategoryRepository.GetAsync(id);
        }


        public async Task<DeviceCategory> GetOrCreateAsync(string deviceCategoryName)
        {
            var deviceCategory = await GetByNameAsync(deviceCategoryName) ??
                                 new DeviceCategory { DeviceCategoryName = deviceCategoryName };

            if (deviceCategory.IsDeleted)
            {
                deviceCategory.IsDeleted = false;
            }

            return _deviceCategoryRepository.InsertOrUpdate(deviceCategory);
        }


        public async Task DeleteAsync(int id)
        {
            var deviceCategory = await _deviceCategoryRepository.FirstOrDefaultAsync(id);

            if (deviceCategory == null)
            {
                return;
            }

            if (deviceCategory.DeviceModels.Count != 0)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("CannotDeleteDeviceCategoryBecauseIsBeingUsed"));
            }

            if (deviceCategory.IsActive)
            {
                deviceCategory.IsActive = false;
            }
            else
            {
                await _deviceCategoryRepository.DeleteAsync(id);
            }
        }


        public async Task RestoreAsync(int id)
        {
            var deviceCategory = await _deviceCategoryRepository.FirstOrDefaultAsync(id);

            if (deviceCategory == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceCategoryNotFound"));
            }

            deviceCategory.IsActive = true;
        }
    }
}
