using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Repositories.Interfaces;
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWorkFactory _uowFactory;
        private readonly IProductsRepository _productRepository;

        public ProductController(IUnitOfWorkFactory uowFactory, IProductsRepository productRepository)
        {
            this._uowFactory = uowFactory;
            this._productRepository = productRepository;
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await this._productRepository.GetByIDAsync(id);
            return Ok(data);
        }

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Products prod)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            using (var uow = this._uowFactory.Create())
            {
                await this._productRepository.AddAsync(prod);
                uow.SaveChanges();
            }

            return NoContent();
        }

    }
}