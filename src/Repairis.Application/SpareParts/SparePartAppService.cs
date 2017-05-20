using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repairis.Brands;
using Repairis.Devices;
using Repairis.SpareParts.Dto;

namespace Repairis.SpareParts
{
    public class SparePartAppService : ApplicationService, ISparePartAppService
    {
        private readonly IRepository<SparePart> _sparePartRepository;
        private readonly IRepository<Device> _deviceRepository;

        private readonly IBrandAppService _brandAppService;

        public SparePartAppService(IRepository<SparePart> sparePartRepository,
            IBrandAppService brandAppService, IRepository<Device> deviceRepository)
        {
            _sparePartRepository = sparePartRepository;
            _brandAppService = brandAppService;
            _deviceRepository = deviceRepository;
        }

        public async Task<List<SparePartBasicEntityDto>> GetCompatibleSpareParts(int deviceId)
        {
            var device = await _deviceRepository.GetAll().Include(x => x.DeviceModel.CompatibleSpareParts).Where(x => x.Id == deviceId)
                .FirstOrDefaultAsync();

            var sparePartIds = device.DeviceModel.CompatibleSpareParts.Select(x => x.SparePartId);
            var spareParts = _sparePartRepository.GetAll().Where(x => sparePartIds.Contains(x.Id)).MapTo<List<SparePartBasicEntityDto>>();

            return spareParts;
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
