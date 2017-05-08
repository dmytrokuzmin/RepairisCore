using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Repairis.MultiTenancy.Dto;

namespace Repairis.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        ListResultDto<TenantListDto> GetTenants();

        Task CreateTenant(CreateTenantInput input);
    }
}
