using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<DeviceModelBasicEntityDto> GetDeviceModelAsync(int id)
        {
            return await _deviceModelRepository
                .GetAll()
                .Where(x => x.Id == id)
                .ProjectTo<DeviceModelBasicEntityDto>()
                .FirstOrDefaultAsync();
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


        public async Task<List<DeviceModelBasicEntityDto>> GetAllDeviceModelsAsync()
        {
            return await _deviceModelRepository.GetAll().ProjectTo<DeviceModelBasicEntityDto>().ToListAsync();
        }

        public async Task<List<DeviceModelAutocompleteDto>> GetAllDeviceModelsForAutocompleteAsync()
        {
            return await _deviceModelRepository.GetAll().ProjectTo<DeviceModelAutocompleteDto>().OrderBy(x => x.DeviceModelName).ToListAsync();
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
