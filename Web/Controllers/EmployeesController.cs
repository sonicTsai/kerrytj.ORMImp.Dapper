using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesController(IEmployeesRepository employeesRepository)
        {
            this._employeesRepository = employeesRepository;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await this._employeesRepository.GetAllAsync();
            return Ok(data);
        }

        // GET: api/Employees/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await this._employeesRepository.GetByIDAsync(id);
            return Ok(data);
        }
        // GET: api/Employees/GetEmployeeSalesByCountry
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetEmployeeSalesByCountry() {
            var data = await this._employeesRepository.GetEmployeeSalesByCountry();
            return Ok(data);
        }

        // GET: api/Employees/GetAllProductWithCategory
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductWithCategory()
        {
            var data = await this._employeesRepository.GetAllProductWithCategory();
            return Ok(data);
        }

        // GET: api/Employees/GetEmployeeSalesByCountry
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GenerateAllTables()
        {
            var data = await this._employeesRepository.GetEmployeeSalesByCountry();
            return Ok(data);
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employees emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await this._employeesRepository.AddAsync(emp);
            return NoContent();
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult>Put(int id, [FromBody] Employees emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await this.GetEmployees(id, emp);
            await this._employeesRepository.UpdateAsync(model);
            return NoContent();
        }

        // PUT: api/Employees/ts/5
        [HttpPut("ts/{id}", Name = "Put")]
        public async Task<IActionResult> PutUsingTransactionScope(int id, [FromBody] Employees emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await this.GetEmployees(id, emp);
            await this._employeesRepository.UpdateUsingTransactionScopeAsync(model);
            return NoContent();
        }

        private async ValueTask<Employees> GetEmployees(int id, Employees emp) {
            var model = await this._employeesRepository.GetByIDAsync(id);
            //在同步的方法中呼叫非同步方法，取得Task<Value>中的Value
            //https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/async/async-return-types
            //Result 屬性是封鎖的屬性。 如果您嘗試在其工作完成之前先存取它，目前使用中的執行緒會封鎖，直到工作完成並且有可用的值為止。 
            //在大部分情況下，您應該使用 await 來存取值，而不是直接存取屬性。
            //var model = this._employeesRepository.GetByIDAsync(id).Result;
            model.LastName = emp.LastName;
            model.FirstName = emp.FirstName;
            model.Title = emp.Title;
            model.TitleOfCourtesy = emp.TitleOfCourtesy;
            model.BirthDate = emp.BirthDate;
            model.HireDate = emp.HireDate;
            model.Address = emp.Address;
            model.City = emp.City;
            model.Region = emp.Region;
            model.PostalCode = emp.PostalCode;
            model.Country = emp.Country;
            model.HomePhone = emp.HomePhone;
            model.Extension = emp.Extension;
            model.Photo = emp.Photo;
            model.Notes = emp.Notes;
            model.ReportsTo = emp.ReportsTo;
            model.PhotoPath = emp.PhotoPath;
            return model;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(int id)
        {
            await this._employeesRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
