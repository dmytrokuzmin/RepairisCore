using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.Roles.Dto;

namespace Repairis.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
