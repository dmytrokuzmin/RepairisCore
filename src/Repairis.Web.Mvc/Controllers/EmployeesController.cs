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
using Repairis.Users;
using Repairis.Users.Dto;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;

namespace Repairis.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Employees)]
    public class EmployeesController : RepairisControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IRepository<EmployeeInfo, long> _employeeInfoRepository;


        public EmployeesController(IUserAppService userAppService, IRepository<EmployeeInfo, long> employeeInfoRepository)
        {
            _userAppService = userAppService;
            _employeeInfoRepository = employeeInfoRepository;
        }

        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        [Route("/api/Employees/")]
        [DontWrapResult]
        public ActionResult EmployeesDataSource([FromBody] DataManager dm)
        {
            var employeesQueryable = _employeeInfoRepository.GetAll();
            int count = employeesQueryable.AsQueryable().Count();
            IEnumerable data = employeesQueryable.ProjectTo<EmployeeBasicEntityDto>();
            DataOperations operation = new DataOperations();
            data = operation.Execute(data, dm);

            return Json(new { result = data.ToDynamicList(), count = count }, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            });
        }


        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        [DisableValidation]
        public async Task<ActionResult> Create_Post(EmployeeInput input)
        {
            if (input.SalaryValue < 0)
            {
                ModelState.AddModelError(nameof(input.SalaryValue), L("SalaryValueMustBeNonNegativeNumber"));
            }

            if (!input.SalaryIsFlat && input.SalaryValue > 100)
            {
                ModelState.AddModelError(nameof(input.SalaryValue), L("SalaryValueMustBeBetween0And100"));
            }

            if (ModelState.IsValid)
            {     
                await _userAppService.CreateEmployeeAsync(input);
                return RedirectToAction("Index");
            }

            return View(input);
        }


        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _employeeInfoRepository.GetAll().Where(x => x.Id == id).ProjectTo<EmployeeBasicEntityDto>().FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeBasicEntityDto input)
        {
            if (input.SalaryValue < 0)
            {
                ModelState.AddModelError(nameof(input.SalaryValue), L("SalaryValueMustBeNonNegativeNumber"));
            }

            if (!input.SalaryIsFlat && input.SalaryValue > 100)
            {
                ModelState.AddModelError(nameof(input.SalaryValue), L("SalaryValueMustBeBetween0And100"));
            }

            if (ModelState.IsValid)
            {
                var employee = await _employeeInfoRepository.GetAllIncluding(x => x.EmployeeUser).FirstOrDefaultAsync(x => x.Id == input.Id);

                if (employee == null)
                {
                    return NotFound();
                }

                employee.EmployeeUser.Address = input.Address;
                employee.EmployeeUser.Name = input.Name;
                employee.EmployeeUser.Surname = input.Surname;
                employee.EmployeeUser.FatherName = input.FatherName;
                employee.EmployeeUser.EmailAddress = input.EmailAddress;
                employee.EmployeeUser.PhoneNumber = input.PhoneNumber;
                employee.EmployeeUser.SecondaryPhoneNumber = input.SecondaryPhoneNumber;
                employee.SalaryIsFlat = input.SalaryIsFlat;
                employee.SalaryValue = input.SalaryValue;


                await _employeeInfoRepository.UpdateAsync(employee);

                return RedirectToAction("Index");
            }
            return View(input);
        }


        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = await _employeeInfoRepository.GetAll().Where(x => x.Id == id).ProjectTo<EmployeeBasicEntityDto>().FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await _userAppService.DeleteEmployeeAsync(id);
            return RedirectToAction("Index");
        }
    }
}
