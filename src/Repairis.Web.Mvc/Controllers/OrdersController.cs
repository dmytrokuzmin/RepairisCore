using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;
using Repairis.Controllers;
using Repairis.Orders;
using Repairis.Orders.Dto;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Repairis.Users;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.Pdf;
using System.Linq.Dynamic.Core;
using Abp.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Authorization;
using Repairis.Authorization.Users;
using Repairis.Users.Dto;
using Repairis.Web.Helpers;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Orders)]
    public class OrdersController : RepairisControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<SparePart, int> _sparePartRepository;
        private readonly IRepository<EmployeeInfo, long> _employeeRepository;
        private readonly IUserAppService _userAppService;
        private readonly ISparePartDomainService _sparePartDomainService;
        private readonly IInvoiceHelper _invoiceHelper;

        public OrdersController(IOrderAppService orderAppService, IRepository<Order, long> orderRepository,
            IUserAppService userAppService, ISparePartDomainService sparePartDomainService, IRepository<SparePart, int> sparePartRepository, IRepository<EmployeeInfo, long> employeeRepository, IInvoiceHelper invoiceHelper)
        {
            _orderAppService = orderAppService;
            _orderRepository = orderRepository;
            _userAppService = userAppService;
            _sparePartDomainService = sparePartDomainService;
            _sparePartRepository = sparePartRepository;
            _employeeRepository = employeeRepository;
            _invoiceHelper = invoiceHelper;
        }

        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }


        [Route("/api/Orders/")]
        [DontWrapResult]
        public ActionResult OrdersDataSource([FromBody] DataManager dm)
        {
            var ordersQueryable = _orderRepository.GetAll();
            int count = ordersQueryable.AsQueryable().Count();
            IEnumerable data = ordersQueryable.ProjectTo<OrderBasicEntityDto>();
            DataOperations operation = new DataOperations();
            data = operation.Execute(data, dm);

            return Json(new { result = data.ToDynamicList(), count = count },  new JsonSerializerSettings
            {   
                ContractResolver = new DefaultContractResolver()
            });
        }


        [DontWrapResult]
        public ActionResult GetOrderSpareParts([FromQuery]long orderId)
        {
            var order = _orderRepository.GetAllIncluding(x => x.SparePartsUsed).FirstOrDefault(x => x.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var spareParts = order.SparePartsUsed.MapTo<List<SparePartOrderMappingDto>>();

            return Json(new {result = spareParts, count = spareParts.Count}, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        [DontWrapResult]
        public ActionResult SparePartUpdate([FromBody]CRUDModel<SparePartOrderMapping> value)
        {
            var action = value.Action;
            var mapping = value.Value;

            if (action == "insert")
                _sparePartDomainService.AddMapping(mapping.SparePartId, mapping.OrderId, mapping.Quantity,
                    mapping.PricePerItem);
            else if (action == "update")
            {
                _sparePartDomainService.UpdateMapping(mapping.SparePartId, mapping.OrderId, mapping.Quantity,
                    mapping.PricePerItem);
            }
            else
            {
                int sparePartId = Int32.Parse(Request.Headers["sparepartid"]);
                int orderId = Int32.Parse(Request.Headers["orderid"]);
                _sparePartDomainService.RemovePartsFromOrder(sparePartId, orderId, null);
            }

            return Json(value, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        public async Task<ActionResult> Create_Post(CreateOrderInput input)
        {
            if (ModelState.IsValid)
            {
                await _orderAppService.CreateOrderAsync(input);
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var orderDto = await _orderAppService.GetOrderDtoAsync(id.Value);
            if (orderDto == null)
            {
                return NotFound();
            }
            var employeeList = await _employeeRepository.GetAll().ProjectTo<EmployeeDropDownListDto>()
                    .OrderBy(x => x.FullName).ToListAsync();
            employeeList.Insert(0, new EmployeeDropDownListDto
            {
                Id = 0,
                Name = ""
            });

            ViewBag.Employees = employeeList;
            return View(orderDto);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderFullEntityDto input)
        {
            bool orderStatusChanged = false;
            var order = _orderRepository.Get(input.Id);
            var previousOrderStatus = order.OrderStatus;
            if (input.OrderStatus != previousOrderStatus)
            {
                orderStatusChanged = true;
            }

            if (input.OrderStatus != OrderStatusEnum.Open && input.OrderStatus != OrderStatusEnum.Waiting)
            {
                if (input.AssignedEmployeeId == 0)
                {
                    ModelState.AddModelError(nameof(input.AssignedEmployeeId), L("PleaseSetAssignedEmployee"));
                }

                if (input.OrderStatus != OrderStatusEnum.InProgress)
                {
                    if (input.RepairPrice == null)
                    {
                        ModelState.AddModelError(nameof(input.RepairPrice), L("PleaseSetRepairPrice"));
                    }
                    if (string.IsNullOrEmpty(input.WorkDoneDescripton))
                    {
                        ModelState.AddModelError(nameof(input.WorkDoneDescripton), L("PleaseFillWorkDoneDescription"));
                    }
                }
            }


            if (ModelState.IsValid)
            {             
                order.OrderStatus = input.OrderStatus;
                order.IsUrgent = input.IsUrgent;
                order.IsWarrantyComplaint = input.IsWarrantyComplaint;
                order.IssueDescription = input.IssueDescription;
                order.AdditionalEquipment = input.AdditionalEquipment;
                order.AdditionalNotes = input.AdditionalNotes;
                order.AssignedEmployeeId = input.AssignedEmployeeId == 0 ? (long?) null : input.AssignedEmployeeId;
                order.RepairPrice = input.RepairPrice;
                order.WorkDoneDescripton = input.WorkDoneDescripton;
                order.DevicePickupDate = input.DevicePickupDate;


                if (orderStatusChanged)
                {
                    if (input.OrderStatus == OrderStatusEnum.Ready)
                    {
                        order.OrderRepairedDate = DateTime.Now;
                        await _orderAppService.NotifyOrderIsReady(order.Id);
                    }
                    else if (previousOrderStatus == OrderStatusEnum.Ready)
                    {
                        await _orderAppService.NotifyOrderIsReturnedToInProgress(order.Id);
                    }
                }

                await _orderRepository.InsertOrUpdateAsync(order);


                return RedirectToAction("Index");
            }

            ViewBag.Employees = await _employeeRepository.GetAll().ProjectTo<EmployeeDropDownListDto>()
                .OrderBy(x => x.FullName).ToListAsync();
            return View(input);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Order order = await _orderRepository.GetAsync((long)id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }


        public List<SparePartBasicEntityDto> GetCompatibleSpareParts(int deviceModelId)
        {
            var spareParts = _sparePartRepository.GetAllList();//.Where(x => x.CompatibleDeviceModels.Any(y => y.DeviceModelId == deviceModelId));
            return spareParts.MapTo<List<SparePartBasicEntityDto>>();
        }

        public ActionResult GetCompatibleSpareParts(int deviceModelId, DataManager dm)
        {
            var spareParts = _sparePartRepository.GetAll();//.Where(x => x.CompatibleDeviceModels.Any(y => y.DeviceModelId == deviceModelId));
            IEnumerable data = spareParts.ProjectTo<List<SparePartBasicEntityDto>>();
            DataOperations operation = new DataOperations();
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                data = operation.PerformSorting(data, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                data = operation.PerformWhereFilter(data, dm.Where, dm.Where[0].Operator);
            }
            int count = data.Cast<SparePartBasicEntityDto>().Count();
            if (dm.Skip != 0)
            {
                data = operation.PerformSkip(data, dm.Skip);
            }
            if (dm.Take != 0)
            {
                data = operation.PerformTake(data, dm.Take);
            }
            return Json(new { result = data, count = count });
        }


        // GET: Orders/Complete/5
        public async Task<ActionResult> Complete(long id)
        {
            var orderCompletion = await _orderRepository
                .GetAll()
                .Where(x => x.Id == id && x.OrderStatus != OrderStatusEnum.Closed &&
                            x.OrderStatus != OrderStatusEnum.OnWarranty)
                .ProjectTo<OrderCompletionDto>()
                .FirstOrDefaultAsync();

            if (orderCompletion == null)
            {
                return NotFound();
            }
            orderCompletion.DevicePickupDate = DateTime.Now;
            return View(orderCompletion);
        }


        // POST: Orders/Complete/5
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteConfirmed(OrderCompletionDto input)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.Get(input.Id);
                order.DevicePickupDate = input.DevicePickupDate;
                if (input.WarrantyMonths > 0)
                {
                    order.OrderStatus = OrderStatusEnum.OnWarranty;
                    order.WarrantyExpirationDate = input.DevicePickupDate.AddMonths(input.WarrantyMonths);
                }
                else
                {
                    order.OrderStatus = OrderStatusEnum.Closed;
                }
                await _orderRepository.UpdateAsync(order);
                return RedirectToAction("FinalInvoice", new { @id = order.Id });
            }

            return View(input);
        }

        public async Task<ActionResult> DeviceReceipt(long id)
        {
            string directory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\invoices\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string currentCultureCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string filePath = $"{directory}\\DeviceReceipt_{id}_{currentCultureCode}.pdf";
            if (!System.IO.File.Exists(filePath))
            {
                var order = await _orderAppService.GetOrderDtoAsync(id);

                // Create a new PdfDocument
                PdfDocument document = _invoiceHelper.GenerateDeviceReceipt(order);

                // Save the document
                document.Save(filePath);
            }

            return new PhysicalFileResult(filePath, "application/pdf");
        }


        public async Task<ActionResult> FinalInvoice(long id)
        {
            string directory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\invoices\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string currentCultureCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string filePath = $"{directory}\\FinalInvoice_{id}_{currentCultureCode}.pdf";                
            if (!System.IO.File.Exists(filePath))
            {
                var order = await _orderAppService.GetOrderDtoAsync(id);

                PdfDocument document = _invoiceHelper.GenerateFinalInvoice(order);

                // Save the document
                document.Save(filePath);
            }

            return new PhysicalFileResult(filePath, "application/pdf");
        }
    }
}
