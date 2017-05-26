using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Authorization.Users;
using Repairis.Controllers;
using Repairis.Orders;
using Repairis.Orders.Dto;
using Repairis.Users.Dto;
using Repairis.Web.Helpers;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ReportsController : RepairisControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<EmployeeInfo, long> _employeeRepository;

        public ReportsController(IOrderAppService orderAppService, IRepository<Order, long> orderRepository, IRepository<EmployeeInfo, long> employeeRepository)
        {
            _orderAppService = orderAppService;
            _orderRepository = orderRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<ActionResult> CompanySalesReport()
        {
            ViewBag.Years = await GetYearsForReport();

            return View();     
        }

        public async Task<ActionResult> CompanyOrdersReport()
        {
            ViewBag.Years = await GetYearsForReport();

            return View();
        }

        public async Task<ActionResult> EmployeesSalaryReport()
        {
            ViewBag.Years = await GetYearsForReport();
            ViewBag.Employees = await GetEmployeesForReport();

            return View();
        }


        [Route("/api/SalesReportDatasource/")]
        [DontWrapResult]
        public ActionResult SalesReportDataSource()
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
                .GroupBy(x => x.DevicePickupDate.Value.Month)
                .Select(x => new SalesReportChartData
                    {
                        Date = new DateTime(selectedYear, x.Key, 1),
                        RepairTotalPrice = x.Sum(i => i.RepairPrice),
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
                            SparePartsTotalSupplierCost = 0
                    }
                    );
                }
            }

            return Json(chartData.OrderBy(x => x.Date).ToList(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }

        [Route("/api/CompletedOrdersReportDatasource/")]
        [DontWrapResult]
        public ActionResult CompletedOrdersReportDataSource()
        {
            var yearHeader = Request.Headers["year"];
            int selectedYear;

            if (string.IsNullOrEmpty(yearHeader) || !int.TryParse(yearHeader[0], out selectedYear))
            {
                selectedYear = DateTime.Now.Year;
            }

            var orders = _orderRepository.GetAll()
                .Where(x => (x.OrderStatus == OrderStatusEnum.Closed || x.OrderStatus == OrderStatusEnum.OnWarranty) && x.DevicePickupDate != null && x.DevicePickupDate.Value.Year == selectedYear)
                .Select(x => new { DevicePickupDate = x.DevicePickupDate.Value, x.Id}).ToList();

            var dataFromDb = orders
                .GroupBy(x => x.DevicePickupDate.Month)
                .Select(x => new OrdersReportChartData
                    {
                        Date = new DateTime(selectedYear, x.Key, 1),
                        CompletedOrdersCount = x.Count()
                    }
                ).ToList();

            var chartData = new List<OrdersReportChartData>();

            for (int i = 1; i <= 12; i++)
            {
                var existing = dataFromDb.FirstOrDefault(x => x.Date.Month == i);

                if (existing != null)
                {
                    chartData.Add(existing);
                }
                else
                {
                    chartData.Add(new OrdersReportChartData
                    {
                            Date = new DateTime(selectedYear, i, 1),
                            CompletedOrdersCount = 0
                    }
                    );
                }
            }

            return Json(chartData.OrderBy(x => x.Date).ToList(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }

        [Route("/api/NewOrdersReportDatasource/")]
        [DontWrapResult]
        public ActionResult NewOrdersReportDataSource()
        {
            var yearHeader = Request.Headers["year"];
            int selectedYear;

            if (string.IsNullOrEmpty(yearHeader) || !int.TryParse(yearHeader[0], out selectedYear))
            {
                selectedYear = DateTime.Now.Year;
            }

            var orders = _orderRepository.GetAll()
                .Where(x => x.CreationTime.Year == selectedYear)
                .Select(x => new { x.CreationTime, x.Id }).ToList();

            var dataFromDb = orders
                .GroupBy(x => new { x.CreationTime.Month })
                .Select(x => new OrdersReportChartData
                    {
                        Date = new DateTime(selectedYear, x.Key.Month, 1),
                        NewOrdersCount = x.Count()
                    }
                ).ToList();

            var chartData = new List<OrdersReportChartData>();

            for (int i = 1; i <= 12; i++)
            {
                var existing = dataFromDb.FirstOrDefault(x => x.Date.Month == i);

                if (existing != null)
                {
                    chartData.Add(existing);
                }
                else
                {
                    chartData.Add(new OrdersReportChartData
                        {
                            Date = new DateTime(selectedYear, i, 1),
                            CompletedOrdersCount = 0
                        }
                    );
                }
            }

            return Json(chartData.OrderBy(x => x.Date).ToList(), new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        [Route("/api/EmployeesSalaryReportDatasource/")]
        [DontWrapResult]
        public async Task<ActionResult> EmployeesSalaryReportDatasource() 
        {
            var yearHeader = Request.Headers["year"];
            var employeeIdHeader = Request.Headers["employeeId"];
            int selectedYear;

            if (string.IsNullOrEmpty(yearHeader) || !int.TryParse(yearHeader[0], out selectedYear))
            {
                selectedYear = DateTime.Now.Year;
            }

            long employeeId;
            if (string.IsNullOrEmpty(employeeIdHeader) || !long.TryParse(employeeIdHeader[0], out employeeId))
            {
                employeeId = (await GetEmployeesForReport()).FirstOrDefault()?.Id ?? 0;
            }

            List<SalaryReportChartData> dataFromDb = new List<SalaryReportChartData>();
            decimal defaultSalaryValue = 0;
            var employee = await _employeeRepository.FirstOrDefaultAsync(employeeId);
            if (employee != null)
            {
                if (employee.SalaryIsFlat)
                {
                    defaultSalaryValue = employee.SalaryValue;
                }
                else
                {
                    var orders = _orderRepository.GetAll().Include(x => x.SparePartsUsed).ThenInclude(x => x.SparePart)
                        .Where(x => x.AssignedEmployeeId == employeeId && x.DevicePickupDate != null &&
                                    x.DevicePickupDate.Value.Year == selectedYear &&
                                    (x.OrderStatus == OrderStatusEnum.Closed ||
                                     x.OrderStatus == OrderStatusEnum.OnWarranty))
                        .MapTo<List<OrderReportItemDto>>();

                    dataFromDb = orders.Where(x => x.DevicePickupDate != null)
                        .GroupBy(x => x.DevicePickupDate.Value.Month)
                        .Select(x => new SalaryReportChartData
                            {
                                Date = new DateTime(selectedYear, x.Key, 1),
                                SalaryValue = Math.Round((x.Sum(i => i.RepairPrice) + x.Sum(i => i.SparePartsTotalCost) -
                                                          x.Sum(i => i.SparePartsTotalSupplierCost)) * employee.SalaryValue / 100)          ,
                            }
                        ).ToList();
                }
            }

            var chartData = new List<SalaryReportChartData>();

            for (int i = 1; i <= 12; i++)
            {
                var existing = dataFromDb.FirstOrDefault(x => x.Date.Month == i);

                if (existing != null)
                {
                    chartData.Add(existing);
                }
                else
                {
                    chartData.Add(new SalaryReportChartData
                    {
                            Date = new DateTime(selectedYear, i, 1),                         
                            SalaryValue = defaultSalaryValue
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

        private async Task<List<EmployeeDropDownListDto>> GetEmployeesForReport()
        {
            return await _employeeRepository.GetAll().ProjectTo<EmployeeDropDownListDto>().ToListAsync();
        }
    }
}