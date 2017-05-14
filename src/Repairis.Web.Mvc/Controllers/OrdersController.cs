using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Repairis.Controllers;
using Repairis.Orders;
using Repairis.Orders.Dto;
using Repairis.SpareParts;
using Repairis.SpareParts.Dto;
using Repairis.Users;
using Repairis.Web.Helpers;
using Syncfusion.Drawing;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize()]
    public class OrdersController : RepairisControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<SparePart, int> _sparePartRepository;
        private readonly IUserAppService _userAppService;
        private readonly ISparePartDomainService _sparePartDomainService;

        public OrdersController(IOrderAppService orderAppService, IRepository<Order, long> orderRepository,
            IUserAppService userAppService, ISparePartDomainService sparePartDomainService, IRepository<SparePart, int> sparePartRepository)
        {
            _orderAppService = orderAppService;
            _orderRepository = orderRepository;
            _userAppService = userAppService;
            _sparePartDomainService = sparePartDomainService;
            _sparePartRepository = sparePartRepository;
        }

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var orders = await _orderAppService.GetAllActiveOrdersAsync();
            return View(orders);
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
            if (id == null)
            {
                return BadRequest();
            }

            var orderDto = await _orderAppService.GetOrderDtoAsync((long)id);
            if (orderDto == null)
            {
                return NotFound();
            }
            var list = _sparePartRepository.GetAllList();
            ViewBag.datasource2 = list.MapTo<List<SparePartBasicEntityDto>>();

            return View(orderDto);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderFullEntityDto input)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.Get(input.Id);

                if (order.OrderStatus != input.OrderStatus)
                {
                    order.OrderStatus = input.OrderStatus;
                    //await _orderAppService.NotifyOrderStatusHasChanged(order.Id);
                }

                order.IsUrgent = input.IsUrgent;
                order.IsWarrantyComplaint = input.IsWarrantyComplaint;
                order.IssueDescription = input.IssueDescription;
                order.AdditionalEquipment = input.AdditionalEquipment;
                order.AdditionalNotes = input.AdditionalNotes;
                order.AssignedEmployeeId = input.AssignedEmployeeId;
                order.IsRepaired = input.IsRepaired;
                order.OrderRepairedDate = input.OrderRepairedDate;
                order.RepairPrice = input.RepairPrice;
                order.WorkDoneDescripton = input.WorkDoneDescripton;
                order.DevicePickupDate = input.DevicePickupDate;

                //var updatedSparePartsUsed = input.SparePartsUsed
                //                   .Where(x => x.OrderId == order.Id)
                //                   .GroupBy(x => new { x.OrderId, x.SparePartId })
                //                   .Select(g => new SparePartOrderMapping
                //                   {
                //                       OrderId = g.Key.OrderId,
                //                       SparePartId = g.Key.SparePartId,
                //                       Quantity = g.Sum(x => x.Quantity),
                //                       PricePerItem = (g.Sum(x => x.Quantity * x.PricePerItem) / g.Sum(x => x.Quantity))
                //                   }).ToList();

                ////handle removed spareparts
                //foreach (var mapping in order.SparePartsUsed)
                //{
                //    var match = updatedSparePartsUsed.FirstOrDefault(x => x.OrderId == mapping.OrderId);

                //    if (match == null)
                //    {
                //        await _sparePartDomainService.RemovePartsFromOrder(mapping.SparePartId, mapping.OrderId, null);
                //    }
                //    else if (match.Quantity < mapping.Quantity)
                //    {
                //        await _sparePartDomainService.RemovePartsFromOrder(mapping.SparePartId, mapping.OrderId, null);
                //    }
                //}

                //foreach (var mapping in updatedSparePartsUsed)
                //{
                //    await
                //        _sparePartDomainService.AddOrUpdateMapping(mapping.SparePartId, mapping.OrderId,
                //            mapping.Quantity, mapping.PricePerItem);
                //}

                await _orderRepository.InsertOrUpdateAsync(order);
                return RedirectToAction("Index");
            }
            //input.OrderDto = await _orderAppService.GetOrderDtoAsync(input.OrderDto.Id);
            //input.Users = await _userAppService.GetUsers();
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
            var spareParts = _sparePartRepository.GetAllList();//.Where(x => x.CompatibleDeviceModels.Any(y => y.DeviceModelId == deviceModelId));
            IEnumerable data = spareParts.MapTo<List<SparePartBasicEntityDto>>();
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


        public void SparePartUpdate(SparePartOrderMappingDto value, string action)
        {
            if (action == "insert")
            {
                _sparePartDomainService.AddMapping(value.SparePartId, value.OrderId, value.Quantity, value.PricePerItem);
            }
            else if (action == "update")
            {
                _sparePartDomainService.UpdateMapping(value.SparePartId, value.OrderId, value.Quantity,
                    value.PricePerItem);
            }
            else
            {
                int sparePartId = Int32.Parse(Request.Headers["sparepartid"]);
                int orderId = Int32.Parse(Request.Headers["orderid"]);

                _sparePartDomainService.RemovePartsFromOrder(sparePartId, orderId, null);
            }

        }


        // GET: Orders/Complete/5
        public async Task<ActionResult> Complete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var order = await _orderRepository.GetAsync((long)id);

            if (order.OrderStatus == OrderStatusEnum.OnWarranty || order.OrderStatus == OrderStatusEnum.Closed)
            {
                throw new UserFriendlyException(L("OrderIsAlreadyCompleted"));
            }
            return View(order.MapTo<OrderCompletionDto>());
        }

        // POST: Orders/Complete/5
        [HttpPost, ActionName("Complete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CompleteConfirmed(OrderCompletionDto input)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepository.Get(input.Id);
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
            var orderCompletionDto = await _orderAppService.GetOrderCompletionDto(input.Id);
            return View(orderCompletionDto);
        }

        public async Task<ActionResult> CreationInvoice(long id)
        {
            var order = await _orderRepository.GetAsync(id);

            // Create a new PdfDocument
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.Pages.Add();

            // Create Pdf graphics for the page
            PdfGraphics graphics = page.Graphics;

            // Create a solid brush
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            // Set the font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20f);

            // Draw the text
            graphics.DrawString(L("OrderCreationInvoice") + " " + order.Id, font, brush, new PointF(20, 20));

            //Export the document after saving     
            return document.ExportAsActionResult("OrderCreation.pdf", Response, HttpReadType.Open);

            //return View();
        }


        public async Task<ActionResult> FinalInvoice(long id)
        {
            var order = await _orderRepository.GetAsync(id);

            // Create a new PdfDocument
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.Pages.Add();

            // Create Pdf graphics for the page
            PdfGraphics graphics = page.Graphics;

            // Create a solid brush
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            // Set the font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20f);

            // Draw the text
            graphics.DrawString(L("OrderFinalInvoice") + " " + order.Id, font, brush, new PointF(20, 20));

            //Export the document after saving     
            return document.ExportAsActionResult("OrderCompletion.pdf", Response, HttpReadType.Open);

            //return View();
        }
    }
}
