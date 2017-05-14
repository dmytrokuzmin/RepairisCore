using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Repairis.Brands;
using Repairis.SpareParts.Dto;

namespace Repairis.SpareParts
{
    public class SparePartAppService : ApplicationService, ISparePartAppService
    {
        private readonly IRepository<SparePart> _sparePartRepository;

        private readonly IBrandAppService _brandAppService;

        public SparePartAppService(IRepository<SparePart> sparePartRepository,
            IBrandAppService brandAppService)
        {
            _sparePartRepository = sparePartRepository;
            _brandAppService = brandAppService;
        }

        public List<SparePartBasicEntityDto> GetCompatibleSpareParts(int deviceModelId)
        {
            var spareParts = _sparePartRepository.GetAllList();//.Where(x => x.CompatibleDeviceModels.Any(y => y.DeviceModelId == deviceModelId));
            return spareParts.MapTo<List<SparePartBasicEntityDto>>();
        }

        public async Task<ListResultDto<SparePartBasicEntityDto>> GetSparePartsAsync()
        {
            var spareParts = await _sparePartRepository.GetAllListAsync();
            return new ListResultDto<SparePartBasicEntityDto>(spareParts.MapTo<List<SparePartBasicEntityDto>>());

        }

        public async Task CreateSparePartAsync(SparePartInput input)
        {
            var brand = await _brandAppService.GetOrCreateAsync(input.BrandName);
            var sparePartId = await _sparePartRepository.InsertAndGetIdAsync(new SparePart
            {
                Brand = brand,
                BrandId = brand.Id,
                SparePartName = input.SparePartName,
                SparePartCode = input.SparePartCode,
                SupplierPrice = input.SupplierPrice,
                RecommendedPrice = input.RecommendedPrice,
                StockStatus = input.StockStatus,
                StockQuantity = input.StockQuantity,
                Notes = input.Notes,
                //CompatibleDevices
            });
        }

        public async Task<SparePartFullEntityDto> GetSparePartDtoAsync(int id)
        {
            var sparePart = await _sparePartRepository.GetAsync(id);

            return sparePart.MapTo<SparePartFullEntityDto>();
        }
    }
}
