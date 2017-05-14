using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Repairis.Orders
{
    public interface IOrderDomainService : IDomainService
    {
        Task<long> CreateAndGetIdAsync(Order order);
        Task DeleteAsync(long id);
        Task RestoreAsync(long id);

    }
}
