using System;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;

namespace Repairis.Orders
{
    public class WarrantyExpirationChecker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Order, long> _orderRepository;

        public WarrantyExpirationChecker(AbpTimer timer, IRepository<Order, long> orderRepository)
            : base(timer)
        {
            _orderRepository = orderRepository;
            Timer.Period = 60 * 60 * 1000; //1 hour
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var now = DateTime.Now;

                var ordersWithExpiredWarranty = 
                    _orderRepository.GetAllList(x => x.OrderStatus == OrderStatusEnum.OnWarranty &&
                                                          x.WarrantyExpirationDate <= now);

                foreach (var order in ordersWithExpiredWarranty)
                {
                    order.OrderStatus = OrderStatusEnum.Closed;
                }

                CurrentUnitOfWork.SaveChanges();
            }
        }
    }
}
