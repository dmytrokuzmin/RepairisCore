using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Localization;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repairis.Devices;
using Repairis.Email;
using Repairis.Helpers;
using Repairis.Orders.Dto;
using Repairis.Sms;
using Repairis.Users;

namespace Repairis.Orders
{
    public class OrderAppService : RepairisAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IDeviceAppService _deviceAppService;
        private readonly IUserAppService _userAppService;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;



        public OrderAppService(IRepository<Order, long> orderRepository,
            IDeviceAppService deviceAppService, IUserAppService userAppService, ISmsService smsService,
            IEmailService emailService)
        {
            _orderRepository = orderRepository;
            _deviceAppService = deviceAppService;
            _userAppService = userAppService;
            _smsService = smsService;
            _emailService = emailService;
        }

        public async Task CreateOrderAsync(CreateOrderInput input)
        {
            var device = await _deviceAppService.GetOrCreateDeviceAsync(input.Device);

            var customer = await _userAppService.GetOrCreateCustomerAsync(input.Customer);

            await _orderRepository.InsertAndGetIdAsync(new Order
            {
                IsUrgent = input.IsUrgent,
                IsWarrantyComplaint = input.IsWarrantyComplaint,
                Device = device,
                DeviceId = device.Id,
                AdditionalEquipment = input.AdditionalEquipment,
                IssueDescription = input.IssueDescription,
                AdditionalNotes = input.AdditionalNotes,
                CustomerId = customer.Id,
            });
        }

        public IQueryable<OrderBasicEntityDto> GetOrdersQueryableDto()
        {
            return _orderRepository.GetAll().ProjectTo<OrderBasicEntityDto>();
        }

        public async Task<OrderBasicListDto> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync();
            return new OrderBasicListDto {Orders = orders.MapTo<List<OrderBasicEntityDto>>()};
        }

        public async Task<OrderBasicListDto> GetAllActiveOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync(x => x.IsActive);
            var mappedOrders = orders.OrderByDescending(x => x.Id).MapTo<List<OrderBasicEntityDto>>();
            return new OrderBasicListDto {Orders = mappedOrders};
        }

        public async Task<OrderBasicListDto> GetAllPassiveOrdersAsync()
        {
            var orders = await _orderRepository.GetAllListAsync(x => !x.IsActive);
            return new OrderBasicListDto {Orders = orders.MapTo<List<OrderBasicEntityDto>>()};
        }


        public async Task<OrderFullEntityDto> GetOrderDtoAsync(long id)
        {
            return await _orderRepository
                .GetAll().Include(x => x.SparePartsUsed).ThenInclude(x => x.SparePart)
                .Where(x => x.Id == id)
                .ProjectTo<OrderFullEntityDto>()
                .FirstOrDefaultAsync();
        }

        public List<DropDownListItem> GetAvailableOrderStatuses(OrderStatusEnum currentStatus)
        {
            var orderStatuses = new List<OrderStatusEnum>();
            if (currentStatus == OrderStatusEnum.Open || currentStatus == OrderStatusEnum.Waiting)
            {
                orderStatuses.Add(OrderStatusEnum.Open);
                orderStatuses.Add(OrderStatusEnum.Waiting);
                orderStatuses.Add(OrderStatusEnum.InProgress);
            }

            if (currentStatus == OrderStatusEnum.InProgress || currentStatus == OrderStatusEnum.Ready)
            {
                orderStatuses.Add(OrderStatusEnum.Open);
                orderStatuses.Add(OrderStatusEnum.Waiting);
                orderStatuses.Add(OrderStatusEnum.InProgress);
                orderStatuses.Add(OrderStatusEnum.Ready);
            }

            return orderStatuses.Select(x => new DropDownListItem {Text = x.GetDisplayName(), Value = x.ToString()})
                .ToList();
        }

        public async Task NotifyOrderIsReady(long orderId)
        {
            var order = await GetOrderDtoAsync(orderId);

            string customerEmail = order.Customer.EmailAddress;
            if (!string.IsNullOrEmpty(customerEmail))
            {
                string subject = L("YourOrderIsRepaired{0}", order.Id);
                string message = L("RepairPriceIs{0}", order.RepairPrice);
                await _emailService.SendEmailAsync(customerEmail, subject, message);
            }

            string customerPhoneNumber = order.Customer.PhoneNumber;
            if (!string.IsNullOrEmpty(customerPhoneNumber))
            {
                string message = $"{L("YourOrderIsRepaired{0}", order.Id)} {L("RepairPriceIs{0}", order.RepairPrice)}";
                await _smsService.SendSmsAsync(customerEmail, message);
            }
        }

        public async Task NotifyOrderIsReturnedToInProgress(long orderId)
        {
            var order = await GetOrderDtoAsync(orderId);

            string customerEmail = order.Customer.EmailAddress;
            if (!string.IsNullOrEmpty(customerEmail))
            {
                string subject = L("YourOrderIsReturnedToInProgress{0}", order.Id);
                string message = $"{L("YourOrderIsReturnedToInProgress{0}", order.Id)}.\n{L("PleaseWaitForFutherEmail")}";
                await _emailService.SendEmailAsync(customerEmail, subject, message);
            }

            string customerPhoneNumber = order.Customer.PhoneNumber;
            if (!string.IsNullOrEmpty(customerPhoneNumber))
            {
                string message = $"{L("YourOrderIsReturnedToInProgress{0}", order.Id)}.\n{L("PleaseWaitForFutherSms")}";
                await _smsService.SendSmsAsync(customerEmail, message);
            }
        }
    }
}
