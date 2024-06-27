using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using exercise1prob2.Models;
using exercise1prob2.Services;

namespace exercise1prob2.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductController(IRepository<Product> repository)
        {
            _repository = repository;
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int skip, [FromQuery]int take)
        {
            var products = await _repository.GetAllAsync(skip, take);
            return Ok(products);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpsertCustomers([FromBody] Product products)
        {
            var upsertedProducts= await _repository.UpsertAsync(products);
            return Ok(upsertedProducts);
        }
    }
}