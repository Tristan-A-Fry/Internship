using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

[Microsoft.AspNetCore.Components.Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        var products = ProductData.GetProducts();
        return Ok(products);
    }

    [HttpPut("products")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult UpdateProduct([FromBody] Product product)
    {
        // Update product logic
        return Ok();
    }
}
