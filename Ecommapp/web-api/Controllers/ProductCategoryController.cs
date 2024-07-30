using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecommapp.Models;
using ecommapp.Services;
using Microsoft.AspNetCore.Authorization;

namespace ecommapp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
       private readonly IRepository<ProductCategory> _repository;

       public ProductCategoryController(IRepository<ProductCategory> repository)
       {
            _repository = repository;
       } 

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories([FromQuery]int skip = 0, [FromQuery]int take = 10) 
        {
            var categories = await _repository.GetAllAsync(skip, take);
            return Ok(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ProductCategory>> UpsertProductCategory([FromBody] ProductCategory categories)
        {
            var upsertedCategories = await _repository.UpsertAsync(categories);
            return Ok(upsertedCategories);
        }

    }
}