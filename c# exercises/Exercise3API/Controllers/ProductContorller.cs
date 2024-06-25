using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Exercise3ConsoleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExampleController : ControllerBase
    {
        [HttpGet("products")]
        [Authorize] // This allows access to authenticated users
        public IActionResult GetProducts()
        {
            var products = ProductData.GetProducts();
            return Ok(products);
        }

        [HttpPut("products")]
        [Authorize(Policy = "AdminPolicy")] // This restricts access to users with the Admin role
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            // Update product logic
            return Ok();
        }
    }

    public static class ProductData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new Product
                {
                    Category = "Electronics",
                    Name = "PlayStation 5",
                    Price = 599.9M
                },
                new Product
                {
                    Category = "Electronics",
                    Name = "Latitude 5540 Laptop",
                    Price = 1289.0M
                },
                new Product
                {
                    Category = "Home & Kitchen",
                    Name = "Dyson V15 Detect Absolute",
                    Price = 749.0M
                },
                new Product
                {
                    Category = "Home & Kitchen",
                    Name = "Ninja Professional Plus 72 fl. oz. Blender with Auto-iQ, Black/Gray",
                    Price = 116.99M
                },
                new Product
                {
                    Category = "Personal Care",
                    Name = "Johnson & Johnson",
                    Price = 5.90M
                },
                new Product
                {
                    Category = "Personal Care",
                    Name = "Gillette Sensor3 Comfort Disposable Razors for Men",
                    Price = 11.97M
                }
            };
        }
    }

    public class Product
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
