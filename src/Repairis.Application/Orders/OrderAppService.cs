using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Localization;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repairis.Brands;
using Repairis.DeviceCategories;
using Repairis.DeviceModels;
using Repairis.Devices;
using Repairis.Helpers;
using Repairis.Orders.Dto;
using Repairis.Users;

namespace Repairis.Orders
{
    public class OrderAppService : RepairisAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IOrderDomainService _orderDomainService;
        private readonly IBrandAppService _brandAppService;
        private readonly IDeviceCategoryAppService _deviceCategoryAppService;
        private readonly IDeviceAppService _deviceAppService;
        private readonly IDeviceModelAppService _deviceModelAppService;
        private readonly IUserAppService _userAppService;


        public OrderAppService(IRepository<Order, long> orderRepository, IOrderDomainService orderDomainService,
            IBrandAppService brandAppService, IDeviceCategoryAppService deviceCategoryAppService,
            IDeviceAppService deviceAppService, IUserAppService userAppService, IDeviceModelAppService deviceModelAppService)
        {
            _orderRepository = orderRepository;
            _orderDomainService = orderDomainService;
            _brandAppService = brandAppService;
            _deviceCategoryAppService = deviceCategoryAppService;
            _deviceAppService = deviceAppService;
            _userAppService = userAppService;
            _deviceModelAppService = deviceModelAppService;
        }

        public async Task CreateOrderAsync(CreateOrderInput input)
        {
            var device = await _deviceAppService.GetOrCreateDeviceAsync(input.Device);

            var customer = await _userAppService.GetOrCreateCustomerAsync(input.Customer);

            var orderId = await _orderRepository.InsertAndGetIdAsync(new Order
            {
                IsUrgent = input.IsUrgent,
                IsWarrantyComplaint = input.IsWarrantyComplaint,
                Device = device,
                DeviceId = device.Id,
                AdditionalEquipment = input.AdditionalEquipment,
                IssueDescription = input.IssueDescription,
                AdditionalNotes = input.AdditionalNotes,
                //Customer = customer,
                CustomerId = customer.Id,
            });


            //TODO: print pdf (+ print customer's password if customer is new)



            //var orderId = await _orderDomainService.CreateAndGetIdAsync(order);

            //try
            //{
            //    var subject = "Your new repair order " + orderId;
            //    subject = subject.Replace('\r', ' ').Replace('\n', ' ');
            //    var body = "Your repair order: ID:" + orderId + " Status:" + order.OrderStatus;
            //    _smtpEmailSender.Send(client.Email, body, subject);
            //}
            //catch (SmtpException)
            //{
            //}

            //Logger.Info("Created a new order: " + orderId + " " + input.DeviceModel.BrandName + " " + input.DeviceModel.DeviceModelName);

        }

        public IQueryable<OrderBasicEntityDto> GetOrdersQueryableDto()
        {
            return _orderRepository.GetAll().ProjectTo<OrderBasicEntityDto>();
        }

        public async Task<OrderBasicListDto> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync();
            return new OrderBasicListDto { Orders = orders.MapTo<List<OrderBasicEntityDto>>() };
        }

        public async Task<OrderBasicListDto> GetAllActiveOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync(x => x.IsActive);
            var mappedOrders = orders.OrderByDescending(x => x.Id).MapTo<List<OrderBasicEntityDto>>();
            return new OrderBasicListDto { Orders = mappedOrders };
        }

        public async Task<OrderBasicListDto> GetAllPassiveOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync(x => !x.IsActive);
            return new OrderBasicListDto { Orders = orders.MapTo<List<OrderBasicEntityDto>>() };
        }


        public async Task<OrderFullEntityDto> GetOrderDtoAsync(long id)
        {
            return await _orderRepository
                .GetAll().Include(x => x.SparePartsUsed).ThenInclude(x => x.SparePart)
                .Where(x => x.Id == id)
                .ProjectTo<OrderFullEntityDto>()
                .FirstOrDefaultAsync();
        }

        public List<DropDownListItem> GetLocalizedOrderStatuses()
        {
            var enumValues = Enum.GetValues(typeof(OrderStatusEnum)).Cast<OrderStatusEnum>();
            return enumValues.Select(x => new DropDownListItem { Text = L(x.GetDisplayName()), Value = x.ToString() }).ToList();
        }

        //public async Task NotifyOrderStatusHasChanged(int id)
        //{

        //var order = await _orderRepository.FirstOrDefaultAsync(id);

        //if (!String.IsNullOrEmpty(order.CustomerUser.EmailAddress))
        //{
        //    string subject;
        //    string body;
        //    if (order.OrderStatus == OrderStatusEnum.Ready)
        //    {
        //        subject = $"Your order (ID: {order.Id}) is repaired";
        //        body = $"Repair price: {order.RepairPrice}";
        //    }
        //    else
        //    {
        //        subject = $"Your order status (ID: {order.Id}) has changed";
        //        body = $"New order status: {order.OrderStatus}";
        //    }
        //    subject = subject.Replace('\r', ' ').Replace('\n', ' ');

        //    try
        //    {
        //        _smtpEmailSender.Send(order.CustomerUser.EmailAddress, body, subject);
        //    }
        //    catch (SmtpException)
        //    {
        //    }
        //}
        //}
    }
}
