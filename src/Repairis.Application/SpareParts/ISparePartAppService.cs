using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Repairis.SpareParts.Dto;

namespace Repairis.SpareParts
{
    public interface ISparePartAppService : IApplicationService
    {
        Task<ListResultDto<SparePartBasicEntityDto>> GetSparePartsAsync();
        Task CreateSparePartAsync(SparePartInput input);
        Task<SparePartFullEntityDto> GetSparePartDtoAsync(int id);
    }
}
