using Microsoft.AspNetCore.Mvc;
using exercise1.Services;
using exercise1.Models;
using System.Collections.Generic;

namespace exercise1.Contorllers
{
[ApiController] [Route("api/v1/[controller]")] public class ProductController : ControllerBase { 
        /* Dependency Injection (DI): ASP.NET Core uses Dependency Injection (DI) to manage the dependencies of classes.
        DI is a design pattern that allows a class to receive its dependencies from an external source rather than creating them itself.

        therefore the value of _productService is an instance of a class that impelements the IProductService interface
        */
        private readonly IProductService _productService;
        public ProductController(IProductService productService) 
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            return Ok(_productService.GetProducts(skip,take));
        }

        [HttpPut]
        public IActionResult UpsertProduct([FromBody] Product product)
        {
            _productService.SaveProduct(product);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct( id);
            return Ok();
        }
    }
}