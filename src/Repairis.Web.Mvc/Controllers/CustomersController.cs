using System.Collections;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Abp.Web.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Repairis.Authorization;
using Repairis.Authorization.Users;
using Repairis.Controllers;
using Repairis.Orders.Dto;
using Repairis.Users;
using Repairis.Users.Dto;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Customers)]
    public class CustomersController : RepairisControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CustomerInfo, long> _customerInfoRepository;


        public CustomersController(IUserAppService userAppService, IRepository<User, long> userRepository, IRepository<CustomerInfo, long> customerInfoRepository)
        {
            _userAppService = userAppService;
            _userRepository = userRepository;
            _customerInfoRepository = customerInfoRepository;
        }

        // GET: Customers
        public  ActionResult Index()
        {
            return View();
        }

        [Route("/api/Customers/")]
        [DontWrapResult]
        public ActionResult CustomersDataSource([FromBody] DataManager dm)
        {
            var customersQueryable = _customerInfoRepository.GetAll(); 
            int count = customersQueryable.AsQueryable().Count();
            IEnumerable data = customersQueryable.ProjectTo<CustomerBasicEntityDto>();
            DataOperations operation = new DataOperations();
            data = operation.Execute(data, dm);

            return Json(new { result = data.ToDynamicList(), count = count }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        public async Task<ActionResult> Create_Post(CustomerInput input)
        {
            if (ModelState.IsValid)
            {
                await _userAppService.CreateCustomerAsync(input);
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = await _customerInfoRepository.GetAll().Where(x => x.Id == id).ProjectTo<CustomerBasicEntityDto>().FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerBasicEntityDto input)
        {
            if (ModelState.IsValid)
            {
                var customer = await _customerInfoRepository.GetAllIncluding(x => x.CustomerUser).FirstOrDefaultAsync(x => x.Id == input.Id);

                if (customer == null)
                {
                    return NotFound();
                }

                customer.AdditionalInfo = input.AdditionalInfo;
                customer.CustomerType = input.CustomerType;
                customer.CustomerUser.Address = input.Address;
                customer.CustomerUser.Name = input.Name;
                customer.CustomerUser.Surname = input.Surname;
                customer.CustomerUser.FatherName = input.FatherName;
                customer.CustomerUser.EmailAddress = input.EmailAddress;
                customer.CustomerUser.PhoneNumber = input.PhoneNumber;
                customer.CustomerUser.SecondaryPhoneNumber = input.SecondaryPhoneNumber;


                await _customerInfoRepository.UpdateAsync(customer);
                //await _userRepository.UpdateAsync(customer.CustomerUser);

                return RedirectToAction("Index");
            }
            return View(input);
        }


        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var customer = await _customerInfoRepository.GetAll().Where(x => x.Id == id).ProjectTo<CustomerBasicEntityDto>().FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }


        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await _userAppService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }
    }
}
