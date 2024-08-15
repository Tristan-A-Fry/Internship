using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;
using Microsoft.AspNetCore.Authorization;
using ecommapp.DTO;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        

        public ProductController(IProductRepository  repository)
        {
            _repository = repository;
        }
        // [Authorize(Roles = "Admin")]
        [HttpGet] 
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] int skip, [FromQuery]int take)
        {
            var products = await _repository.GetProductsAsync(skip, take);
            return Ok(products);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpsertCustomers([FromBody] Product products)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var upsertedProducts= await _repository.UpsertProductAsync(products);
            return Ok(upsertedProducts);
        }
    }
}