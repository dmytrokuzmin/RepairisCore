using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.Sessions.Dto;

namespace Repairis.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
