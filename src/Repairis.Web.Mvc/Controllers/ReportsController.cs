using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Controllers;
using Repairis.Orders;
using Repairis.Orders.Dto;
using Repairis.Web.Helpers;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ReportsController : RepairisControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, long> _orderRepository;

        public ReportsController(IOrderAppService orderAppService, IRepository<Order, long> orderRepository)
        {
            _orderAppService = orderAppService;
            _orderRepository = orderRepository;
        }

        public async Task<ActionResult> Index(int? year)
        {
            ViewBag.Years = await GetYearsForReport();

            return View();     
        }


        [Route("/api/SalesReportDatasource/")]
        [DontWrapResult]
        public ActionResult ReportsDataSource()
        {
            var yearHeader = Request.Headers["year"];
            int selectedYear;

            if(string.IsNullOrEmpty(yearHeader) || !int.TryParse(yearHeader[0], out selectedYear))
            {
                selectedYear = DateTime.Now.Year;
            }

            var orders = _orderRepository.GetAll().Include(x => x.SparePartsUsed).ThenInclude(x => x.SparePart)
                .Where(x => x.OrderStatus == OrderStatusEnum.Closed || x.OrderStatus == OrderStatusEnum.OnWarranty && x.DevicePickupDate != null && x.DevicePickupDate.Value.Year == selectedYear)
                .MapTo<List<OrderReportItemDto>>();

            var dataFromDb = orders.Where(x => x.DevicePickupDate != null)
                .GroupBy(x => new { Month = x.DevicePickupDate.Value.Month })
                .Select(x => new SalesReportChartData
                    {
                        Date = new DateTime(selectedYear, x.Key.Month, 1),
                        RepairTotalPrice = (int)x.Sum(i => i.RepairPrice),
                        SparePartsTotalCost = x.Sum(i => i.SparePartsTotalCost),
                        SparePartsTotalSupplierCost = x.Sum(i => i.SparePartsTotalSupplierCost),
                    }
                ).ToList();

            var chartData = new List<SalesReportChartData>();

            for (int i = 1; i <= 12; i++)
            {
                var existing = dataFromDb.FirstOrDefault(x => x.Date.Month == i);

                if (existing != null)
                {
                    chartData.Add(existing);
                }
                else
                {
                    chartData.Add(new SalesReportChartData
                        {
                            Date = new DateTime(selectedYear, i, 1),
                            RepairTotalPrice = 0,
                            SparePartsTotalCost = 0,
                            SparePartsTotalSupplierCost = 0,
                        }
                    );
                }
            }

            return Json(chartData.OrderBy(x => x.Date).ToList(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        private async Task<List<int>> GetYearsForReport()
        {
            var firstOrder = await _orderRepository.GetAll().OrderBy(x => x.CreationTime).FirstOrDefaultAsync();
            var currentYear = DateTime.Now.Year;
            var years = new List<int>();

            int firstYear = firstOrder == null ? currentYear : firstOrder.CreationTime.Year;
            while (currentYear >= firstYear)
            {
                years.Add(currentYear);
                currentYear--;
            }

            return years;
        }
    }
}