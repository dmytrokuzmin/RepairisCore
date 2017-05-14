using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Repairis.Brands;
using Repairis.DeviceCategories;

namespace Repairis.DeviceModels
{
    public class DeviceModelDomainService : DomainService, IDeviceModelDomainService
    {
        private readonly IRepository<DeviceModel> _deviceModelRepository;
        private readonly IDeviceCategoryDomainService _deviceCategoryDomainService;
        private readonly IBrandDomainService _brandDomainService;

        public DeviceModelDomainService(IRepository<DeviceModel> deviceModelRepository,
            IDeviceCategoryDomainService deviceCategoryDomainService,
            IBrandDomainService brandDomainService)
        {
            _deviceModelRepository = deviceModelRepository;
            _deviceCategoryDomainService = deviceCategoryDomainService;
            _brandDomainService = brandDomainService;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        public async Task<bool> ExistsAsync(string deviceCategoryName, string brandName, string deviceModelName)
        {
            return await FindDeviceModelAsync(deviceCategoryName, brandName, deviceModelName) != null;
        }

        public async Task CreateAsync(DeviceModel deviceModel)
        {
            if (
                !await
                    ExistsAsync(deviceModel.DeviceCategory.DeviceCategoryName, deviceModel.Brand.BrandName, deviceModel.DeviceModelName))
            {
                await _deviceModelRepository.InsertAsync(deviceModel);
            }
        }

        public async Task CreateAsync(string deviceCategoryName, string brandName, string deviceModelName)
        {
            if (await ExistsAsync(deviceCategoryName, brandName, deviceModelName))
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceModelAlreadyExists"));
            }

            var deviceCategory = await _deviceCategoryDomainService.GetOrCreateAsync(deviceCategoryName);
            var brand = await _brandDomainService.GetOrCreateAsync(brandName);

            var deviceModel = new DeviceModel
            {
                DeviceCategory = deviceCategory,
                DeviceCategoryId = deviceCategory.Id,
                Brand = brand,
                BrandId = brand.Id,
                DeviceModelName = deviceModelName
            };

            await _deviceModelRepository.InsertAsync(deviceModel);
        }

        public async Task<int> CreateAndGetIdAsync(string deviceCategoryName, string brandName, string deviceModelName)
        {
            if (await ExistsAsync(deviceCategoryName, brandName, deviceModelName))
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceModelAlreadyExists"));
            }

            var deviceCategory = await _deviceCategoryDomainService.GetOrCreateAsync(deviceCategoryName);
            var brand = await _brandDomainService.GetOrCreateAsync(brandName);

            var deviceModel = new DeviceModel
            {
                DeviceCategory = deviceCategory,
                DeviceCategoryId = deviceCategory.Id,
                Brand = brand,
                BrandId = brand.Id,
                DeviceModelName = deviceModelName
            };

            return await _deviceModelRepository.InsertAndGetIdAsync(deviceModel);
        }

        public async Task<DeviceModel> FindDeviceModelAsync(string deviceCategoryName, string brandName, string deviceModelName)
        {
            return await _deviceModelRepository.FirstOrDefaultAsync(
                d => d.DeviceModelName.ToUpper() == deviceModelName.ToUpper() &&
                     d.DeviceCategory.DeviceCategoryName.ToUpper() == deviceCategoryName &&
                     d.Brand.BrandName.ToUpper() == brandName.ToUpper()
            );
        }

        public async Task<DeviceModel> GetOrCreateAsync(string deviceCategoryName, string brandName, string deviceModelName)
        {
            bool deviceModelMayExist = true;

            //Try to find Device Category
            var deviceCategory = await _deviceCategoryDomainService.GetByNameAsync(deviceCategoryName);

            if (deviceCategory == null)
            {
                deviceModelMayExist = false;
                deviceCategory = await _deviceCategoryDomainService.CreateAsync(deviceCategoryName);
            }

            // restore if deleted
            deviceCategory.IsDeleted = false;
            deviceCategory.IsActive = true;

            //Try to find Brand
            var brand = await _brandDomainService.GetByNameAsync(brandName);

            if (brand == null)
            {
                deviceModelMayExist = false;
                brand = await _brandDomainService.CreateAsync(brandName);
            }

            brand.IsDeleted = false;
            brand.IsActive = true;

            DeviceModel deviceModel = null;

            //Try to find DeviceModel if it may exist
            if (deviceModelMayExist)
            {
                deviceModel = _deviceModelRepository.FirstOrDefault(
                    x =>
                        x.DeviceModelName.ToUpper() == deviceModelName.ToUpper() &&
                        x.BrandId == brand.Id &&
                        x.DeviceCategoryId == deviceCategory.Id
                );
            }


            if (deviceModel != null)
            {
                return deviceModel;
            }

            var id = await _deviceModelRepository.InsertAndGetIdAsync(new DeviceModel
            {
                BrandId = brand.Id,
                Brand = brand,
                DeviceCategory = deviceCategory,
                DeviceCategoryId = deviceCategory.Id,
                DeviceModelName = deviceModelName
            });

            return await _deviceModelRepository.GetAsync(id);
        }





        public async Task<DeviceModel> GetDeviceModelAsync(int id)
        {
            var deviceModel = await _deviceModelRepository.FirstOrDefaultAsync(id);
            if (deviceModel == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceModelNotFound"));
            }
            return deviceModel;
        }

        public async Task DeleteAsync(int id)
        {
            var deviceModel = await _deviceModelRepository.FirstOrDefaultAsync(id);

            if (deviceModel == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceModelNotFound"));
            }

            //if ((deviceModel.Orders.Count != 0))
            //{
            //    throw new UserFriendlyException(LocalizationSource.GetString("CannotDeleteDeviceModelBecauseIsBeingUsed"));
            //}

            if (deviceModel.IsActive)
            {
                deviceModel.IsActive = false;
            }
            else
            {
                await _deviceModelRepository.DeleteAsync(id);
            }
        }

        public async Task RestoreAsync(int id)
        {
            var deviceModel = await _deviceModelRepository.FirstOrDefaultAsync(id);

            if (deviceModel == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceModelNotFound"));
            }

            deviceModel.IsActive = true;
        }
    }
}
