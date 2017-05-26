using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Repairis.Helpers;
using Repairis.Orders.Dto;

namespace Repairis.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task CreateOrderAsync(CreateOrderInput input);
        Task<OrderFullEntityDto> GetOrderDtoAsync(long id);

        IQueryable<OrderBasicEntityDto> GetOrdersQueryableDto();



        Task<OrderBasicListDto> GetAllOrdersAsync();
        Task<OrderBasicListDto> GetAllActiveOrdersAsync();
        Task<OrderBasicListDto> GetAllPassiveOrdersAsync();

        List<DropDownListItem> GetLocalizedOrderStatuses();


        //Task NotifyOrderStatusHasChanged(int id);
    }
}
