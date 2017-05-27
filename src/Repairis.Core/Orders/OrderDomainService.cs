using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Repairis.Authorization.Users;

namespace Repairis.Orders
{
    public class OrderDomainService : DomainService, IOrderDomainService
    {
        private readonly IRepository<Order, long> _orderRepository;

        public OrderDomainService(IRepository<Order, long> orderRepository)
        {
            _orderRepository = orderRepository;
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }

        public void AssignOrderToMaster(Order order, EmployeeInfo master)
        {

            if (order.AssignedEmployeeId == master.Id)
            {
                return;
            }

            if (!IsAllowedToAssign(order))
            {
                throw new UserFriendlyException(LocalizationSource.GetString("NotAllowedToAssignOrder"));

            }

            if (order.OrderStatus == OrderStatusEnum.Open)
            {
                order.OrderStatus = OrderStatusEnum.InProgress;
            }

            order.AssignedEmployeeId = master.Id;
        }

        public bool IsAllowedToAssign(Order order)
        {
            return order.OrderStatus == OrderStatusEnum.Open ||
                   order.OrderStatus == OrderStatusEnum.Waiting ||
                   order.OrderStatus == OrderStatusEnum.InProgress;
        }

        public async Task<long> CreateAndGetIdAsync(Order order)
        {
            return await _orderRepository.InsertAndGetIdAsync(order);
        }

        public async Task DeleteAsync(long id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(id);
            if (order == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("OrderNotFound"));
            }

            if (order.IsActive)
            {
                order.IsActive = false;
            }
            else
            {
                await _orderRepository.DeleteAsync(order.Id);
            }
        }

        public async Task RestoreAsync(long id)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(id);
            if (order == null)
            {
                throw new UserFriendlyException(LocalizationSource.GetString("OrderNotFound"));
            }

            order.IsActive = true;
        }
    }
}
