using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Repairis.DeviceCategories.Dto;

namespace Repairis.DeviceCategories
{
    public class DeviceCategoryAppService : RepairisAppServiceBase, IDeviceCategoryAppService
    {
        private readonly IDeviceCategoryDomainService _deviceCategoryDomainService;
        private readonly IRepository<DeviceCategory> _deviceCategoryRepository;


        public DeviceCategoryAppService(IDeviceCategoryDomainService deviceCategoryDomainService, IRepository<DeviceCategory> deviceCategoryRepository)
        {
            _deviceCategoryDomainService = deviceCategoryDomainService;
            _deviceCategoryRepository = deviceCategoryRepository;
        }


        //Creates device category based on input
        public async Task CreateDeviceCategoryAsync(DeviceCategoryBasicEntityDto input)
        {
            await _deviceCategoryDomainService.CreateAsync(input.MapTo<DeviceCategory>());
        }


        public Task<DeviceCategory> GetOrCreateAsync(string deviceCategoryName)
        {
            return _deviceCategoryDomainService.GetOrCreateAsync(deviceCategoryName);
        }

        public async Task<DeviceCategoryFullEntityDto> GetDeviceCategoryAsync(int id)
        {
            var category = await _deviceCategoryRepository.FirstOrDefaultAsync(id);
            if (category == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("DeviceCategoryNotFound"));
            }
            return category.MapTo<DeviceCategoryFullEntityDto>();
        }


        //public async Task<DeviceCategoryFullEntityDto> GetDeviceCategoryAsync(string deviceCategoryName)
        //{
        //    var category = await _deviceCategoryDomainService.GetByNameAsync(deviceCategoryName);
        //    if (category == null)
        //    {
        //        throw new UserFriendlyException(LocalizationSource.GetString("DeviceCategoryNotFound"));
        //    }
        //    return category.MapTo<DeviceCategoryFullEntityDto>();
        //}


        public async Task<List<DeviceCategoryBasicEntityDto>> GetAllDeviceCategoriesAsync()
        {
            return (await _deviceCategoryRepository.GetAllListAsync()).MapTo<List<DeviceCategoryBasicEntityDto>>();  
        }



        public async Task DeleteAsync(int id)
        {
            await _deviceCategoryDomainService.DeleteAsync(id);
        }

        public async Task RestoreAsync(int id)
        {
            await _deviceCategoryDomainService.RestoreAsync(id);
        }
    }
}
