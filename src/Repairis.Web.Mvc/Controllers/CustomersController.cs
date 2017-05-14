using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using Microsoft.AspNetCore.Mvc;
using Repairis.Authorization.Users;
using Repairis.Controllers;
using Repairis.Users;
using Repairis.Users.Dto;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize]
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
        public async Task<ActionResult> Index()
        {
            var customers = await _userAppService.GetAllActiveCustomersAsync();
            return View(customers);
        }


        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Clients/Create
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


        // GET: Clients/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var customer = await _userAppService.GetCustomerDtoAsync((long)id);

            return View(customer);
        }


        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CustomerFullEntityDto input)
        {
            if (ModelState.IsValid)
            {
                var customer = await _userRepository.GetAsync(input.Id);

                customer.CustomerInfo.AdditionalInfo = input.AdditionalInfo;
                customer.CustomerInfo.CustomerType = input.CustomerType;
                customer.Address = input.Address;
                customer.Name = input.Name;
                customer.Surname = input.Surname;
                customer.FatherName = input.FatherName;
                customer.EmailAddress = input.EmailAddress;
                customer.PhoneNumber = input.PhoneNumber;
                customer.SecondaryPhoneNumber = input.SecondaryPhoneNumber;


                await _userRepository.UpdateAsync(customer);
                await _customerInfoRepository.UpdateAsync(customer.CustomerInfo);

                return RedirectToAction("Index");
            }
            return View(input);
        }


        // GET: Clients/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var customer = await _userAppService.GetCustomerDtoAsync((long)id);
            return View(customer);
        }


        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await _userAppService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }
    }
}
