using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels.Dto;

namespace Repairis.DeviceModels
{
    public class DeviceModelAppService : RepairisAppServiceBase, IDeviceModelAppService
    {
        private readonly IRepository<DeviceModel> _deviceModelRepository;
        private readonly IBrandAppService _brandAppService;
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IDeviceModelDomainService _deviceModelDomainService;

        public DeviceModelAppService(
            IRepository<DeviceModel> deviceModelRepository,
            IBrandAppService brandAppService,
            IDeviceCategoryAppService deviceCategoryAppService,
            IDeviceModelDomainService deviceModelDomainService
        )
        {
            _deviceModelRepository = deviceModelRepository;
            _brandAppService = brandAppService;
            _deviceCategoryAppService = deviceCategoryAppService;
            _deviceModelDomainService = deviceModelDomainService;
        }


        public async Task<DeviceModel> FindDeviceModelAsync(DeviceModelBasicEntityDto input)
        {
            return await _deviceModelDomainService.FindDeviceModelAsync(input.DeviceModelName, input.DeviceCategoryName, input.BrandName);
        }

        public async Task<DeviceModelFullEntityDto> GetDeviceModelAsync(int id)
        {
            var deviceModel = await _deviceModelDomainService.GetDeviceModelAsync(id);
            return deviceModel.MapTo<DeviceModelFullEntityDto>();
        }


        public async Task<bool> ExistsAsync(DeviceModelBasicEntityDto input)
        {
            return await _deviceModelDomainService.ExistsAsync(input.DeviceModelName, input.DeviceCategoryName, input.BrandName);
        }

        public async Task<DeviceModel> GetOrCreateAsync(DeviceModelBasicEntityDto input)
        {
            return await _deviceModelDomainService.GetOrCreateAsync(input.DeviceModelName, input.DeviceCategoryName, input.BrandName);

        }

        public async Task CreateDeviceModelAsync(DeviceModelBasicEntityDto input)
        {
            await
                _deviceModelDomainService.CreateAsync(input.DeviceModelName, input.DeviceCategoryName, input.BrandName);

            Logger.Info("DeviceModels: Created a new device model: " +
                        input.BrandName + " " +
                        input.DeviceModelName + " (" +
                        input.DeviceCategoryName + ")");
        }


        public async Task<DeviceModelViewBagDto> GenerateViewBagDtoAsync()
        {
            var brands = await _brandAppService.GetAllBrandsAsync();
            var categories = await _deviceCategoryAppService.GetAllDeviceCategoriesAsync();
            var deviceModels = await GetAllDeviceModelsAsync();

            return new DeviceModelViewBagDto
            {
                Brands = brands.Brands,
                DeviceCategories = categories.DeviceCategories,
                DeviceModels = deviceModels.DeviceModels
            };
        }


        public async Task<DeviceModelBasicListDto> GetAllDeviceModelsAsync()
        {
            var deviceModels = await _deviceModelRepository.GetAllListAsync();
            var deviceModelsDto = deviceModels.MapTo<List<DeviceModelBasicEntityDto>>();
            var sortedDeviceModelsDto = deviceModelsDto.OrderBy(x => x.DeviceCategoryName).ThenBy(x => x.BrandName).ThenBy(x => x.DeviceModelName).ToList();
            return new DeviceModelBasicListDto { DeviceModels = sortedDeviceModelsDto };
        }


        public async Task<DeviceModelBasicListDto> GetAllActiveDeviceModelsAsync()
        {
            var deviceModels = await _deviceModelRepository.GetAllListAsync(x => x.IsActive);
            var deviceModelsDto = deviceModels.MapTo<List<DeviceModelBasicEntityDto>>();
            var sortedDeviceModelsDto = deviceModelsDto.OrderBy(x => x.DeviceCategoryName).ThenBy(x => x.BrandName).ThenBy(x => x.DeviceModelName).ToList();
            return new DeviceModelBasicListDto { DeviceModels = sortedDeviceModelsDto };
        }


        public async Task<DeviceModelBasicListDto> GetAllPassiveDeviceModelsAsync()
        {
            var deviceModels = await _deviceModelRepository.GetAllListAsync(x => !x.IsActive);
            var deviceModelsDto = deviceModels.MapTo<List<DeviceModelBasicEntityDto>>();
            var sortedDeviceModelsDto = deviceModelsDto.OrderBy(x => x.DeviceCategoryName).ThenBy(x => x.BrandName).ThenBy(x => x.DeviceModelName).ToList();
            return new DeviceModelBasicListDto { DeviceModels = sortedDeviceModelsDto };
        }


        public async Task DeleteAsync(int id)
        {
            await _deviceModelDomainService.DeleteAsync(id);
        }


        public async Task RestoreAsync(int id)
        {
            await _deviceModelDomainService.RestoreAsync(id);
        }
    }
}
