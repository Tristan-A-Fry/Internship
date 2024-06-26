using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPut("suppliers")]
    public IActionResult UpdateSuppliers([FromBody] Supplier supplier)
    {
        // Update supplier logic...
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("productCategories")]
    public IActionResult UpdateProductCategories([FromBody] ProductCategory productCategory)
    {
        // Update product category logic...
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("products")]
    public IActionResult UpdateProducts([FromBody] Product product)
    {
        // Update product logic...
        return Ok();
    }
}

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
    // Add other relevant properties
}

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    // Add other relevant properties
}
